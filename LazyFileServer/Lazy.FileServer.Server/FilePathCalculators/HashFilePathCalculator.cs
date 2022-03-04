using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Server.FilePathCalculators
{
    public class HashFilePathCalculator : IFilePathCalculator
    {
        public string Name
        {
            get { return "hash"; }
        }

        public string Calculate(FilePathCalculatorInput input)
        {
            var md5 = MD5.Create();
            var bytes = md5.ComputeHash(input.Bytes);
            var base64 = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var dir1 = base64.Substring(0, 2);
            var dir2 = base64.Substring(2, 2);
            var ext = Path.GetExtension(input.FileName);
            var fileName = base64 + ext;

            return Path.Combine(dir1, dir2, fileName);
        }
    }
}
