using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazy.FileServer.Server.Exceptions;
using Lazy.FileServer.Server.FilePathCalculators;
using Lazy.FileServer.Server.Options;

namespace Lazy.FileServer.Server
{
    public class DefaultFileServer : IFileServer
    {
        private readonly IFilePathCalculator _filePathCalculator;
        private readonly IAppFinder _appFinder;
        private readonly FileServerOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string APPID = "appid";
        private const string APPKEY = "appkey";

        public DefaultFileServer(
            IOptionsSnapshot<FileServerOptions> options,
            IHttpContextAccessor httpContextAccessor,
            IEnumerable<IFilePathCalculator> filePathCalculators,
            IAppFinder appFinder)
        {
            this._options = options.Value;
            this._httpContextAccessor = httpContextAccessor;
            this._filePathCalculator = FindFilePathCalculator(filePathCalculators, _options.FilePathCalculatorType);
            this._appFinder = appFinder;
        }

        private IFilePathCalculator FindFilePathCalculator(IEnumerable<IFilePathCalculator> filePathCalculators, string name)
        {
            var target = filePathCalculators.Where(e => e.Name == name).FirstOrDefault();
            if (target == null) throw new FilePathCalculatorNotFoundException("未找到对应的FilePathCalculator Provider");
            return target;
        }

        public async Task<string> UploadFileAsync(string fileName, byte[] bytes)
        {
            var appName = (await CheckAuthAsync()).AppName;

            var withAppLocalBase = Path.Combine(_options.LocalBase, appName);
            var filePath = _filePathCalculator.Calculate(new FilePathCalculatorInput(withAppLocalBase, fileName, bytes));
            var fullFilePath = Path.Combine(withAppLocalBase, filePath);

            MakeSureDirectory(fullFilePath);

            using (var fileStream = File.Open(fullFilePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
                await fileStream.FlushAsync();
            }

            return $"{_options.HttpBase}/{appName}/{ToHttpPath(filePath)}";
        }

        private async Task<AppInfo> CheckAuthAsync()
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            var appId = headers[APPID].FirstOrDefault();
            var appKey = headers[APPKEY].FirstOrDefault();
            var app = await _appFinder.FindAsync(appId);

            if (string.IsNullOrWhiteSpace(appId) ||
                string.IsNullOrWhiteSpace(appKey) ||
                app == null ||
                app.AppId != appId ||
                app.AppKey != appKey)
            {
                throw new FobiddenException("禁止访问，无权限");
            }

            return app;
        }

        private void MakeSureDirectory(string fullFilePath)
        {
            var dir = Path.GetDirectoryName(fullFilePath);
            if (string.IsNullOrEmpty(dir))
            {
                throw new Exception("无效目录");
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private string ToHttpPath(string filePath)
        {
            var parts = filePath.Split(Path.DirectorySeparatorChar);
            return String.Join("/", parts);
        }
    }
}
