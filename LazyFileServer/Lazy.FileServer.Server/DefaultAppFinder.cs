using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lazy.FileServer.Server.Options;
using System.Linq;

namespace Lazy.FileServer.Server
{
    public class DefaultAppFinder : IAppFinder
    {
        private readonly FileServerOptions _options;

        public DefaultAppFinder(IOptionsSnapshot<FileServerOptions> options)
        {
            this._options = options.Value;
        }

        public Task<AppInfo> FindAsync(string appid)
        {
            return Task.FromResult(_options.Apps.FirstOrDefault(e => e.AppId == appid));
        }
    }
}
