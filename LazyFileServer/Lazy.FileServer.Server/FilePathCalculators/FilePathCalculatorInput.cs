using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Server.FilePathCalculators
{
    public class FilePathCalculatorInput
    {
        public FilePathCalculatorInput(string localBase, string fileName, byte[] bytes)
        {
            LocalBase = localBase;
            FileName = fileName;
            Bytes = bytes;
        }

        /// <summary>
        /// 基础路径（包含app部分）
        /// </summary>
        public string LocalBase { get; set; }
        /// <summary>
        /// 上传文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 上传文件二进制
        /// </summary>
        public byte[] Bytes { get; set; }
    }
}
