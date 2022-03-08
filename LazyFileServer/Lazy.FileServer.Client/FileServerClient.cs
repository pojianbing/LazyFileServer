using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;

namespace Lazy.FileServer.Client
{
    public class FileServerClient
    {
        private string serverUrl;
        private string appid;
        private string appkey;

        public FileServerClient(string serverUrl, string appid, string appkey)
        {
            this.serverUrl = serverUrl;
            this.appid = appid;
            this.appkey = appkey;
        }

        public async Task<string> UploadOneAsync(HttpContext httpContext)
        {
            var files = httpContext.Request.Form.Files.Select(e => Tuple.Create(e.FileName, e.OpenReadStream()));
            if (files.Count() == 0) throw new Exception("未找到要上传的文件");
            return await UploadOneAsync(files.First().Item1, files.First().Item2);
        }

        public async Task<List<string>> UploadAsync(HttpContext httpContext)
        {
            var files = httpContext.Request.Form.Files.Select(e => Tuple.Create(e.FileName, e.OpenReadStream()));
            if (files.Count() == 0) throw new Exception("未找到要上传的文件");
            return await UploadAsync(files);
        }

        public async Task<string> UploadOneAsync(string fileName, byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return await UploadOneAsync(fileName, stream);
        }

        public async Task<List<string>> UploadAsync(IEnumerable<Tuple<string, byte[]>> files)
        {
            var streamFiles = files.Select(f => Tuple.Create(f.Item1, new MemoryStream(f.Item2) as Stream));
            return await UploadAsync(streamFiles);
        }

        public async Task<string> UploadOneAsync(string fileName, Stream stream)
        {
            var urls = await UploadAsync(new List<Tuple<string, Stream>> { Tuple.Create(fileName, stream) });
            if (urls.Count > 0) return urls[0];

            throw new Exception("返回url数量为空");
        }

        public async Task<List<string>> UploadAsync(IEnumerable<Tuple<string, Stream>> files)
        {
            var reponse = await serverUrl.WithHeader("appid", appid).WithHeader("appkey", appkey).PostMultipartAsync(mp =>
            {
                foreach (var f in files)
                {
                    mp.AddFile(f.Item1, f.Item2, f.Item1);
                }
            });

            if (reponse.StatusCode == 200)
            {
                var serverResponse = await reponse.GetJsonAsync<FileServerResponse>();
                if (serverResponse.Code == 200)
                {
                    return serverResponse.Result;
                }
                else
                {
                    throw new FileServerException(serverResponse.Error);
                }
            }

            throw new FileServerException("文件服务器异常");
        }
    }
}
