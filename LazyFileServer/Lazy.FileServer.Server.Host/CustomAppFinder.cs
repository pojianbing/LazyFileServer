
using Lazy.FileServer.Server.Options;

namespace Lazy.FileServer.Server.Host.Host
{
    public class CustomAppFinder : IAppFinder
    {
        private static List<AppInfo> Apps = new List<AppInfo>()
        {
            new AppInfo{ AppId = "1", AppKey = "654321", AppName = "spider" }
        };

        public Task<AppInfo> FindAsync(string appid)
        {
            return Task.FromResult(Apps.FirstOrDefault(e => e.AppId == appid));
        }
    }
}
