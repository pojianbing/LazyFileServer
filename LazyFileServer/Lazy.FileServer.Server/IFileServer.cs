using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Server
{
    public interface IFileServer
    {
        Task<string> UploadFileAsync(string fileName, byte[] bytes);
    }
}
