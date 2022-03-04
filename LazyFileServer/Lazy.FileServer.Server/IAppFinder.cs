using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lazy.FileServer.Server.Options;

namespace Lazy.FileServer.Server
{
    public interface IAppFinder
    {
        /// <summary>
        /// 查找
        /// 未找到返回null
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        Task<AppInfo> FindAsync(string appid);
    }
}
