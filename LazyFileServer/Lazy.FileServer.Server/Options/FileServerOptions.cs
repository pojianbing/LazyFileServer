using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazy.FileServer.Server.FilePathCalculators;

namespace Lazy.FileServer.Server.Options
{
    public class FileServerOptions
    {
        /// <summary>
        /// 本地基目录
        /// </summary>
        public string LocalBase { get; set; }
        /// <summary>
        /// http服务器基目录
        /// </summary>
        public string HttpBase { get; set; }
        /// <summary>
        /// 路径计算类型 date, hash
        /// </summary>
        public string FilePathCalculatorType { get; set; } = "date";
        /// <summary>
        /// 应用信息
        /// </summary>
        public List<AppInfo> Apps { get; set; }
    }
}
