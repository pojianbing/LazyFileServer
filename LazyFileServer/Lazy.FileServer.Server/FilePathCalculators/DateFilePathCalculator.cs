using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazy.FileServer.Server.Exceptions;

namespace Lazy.FileServer.Server.FilePathCalculators
{
    public class DateFilePathCalculator : IFilePathCalculator
    {
        public string Name
        {
            get { return "date"; }
        }

        public string Calculate(FilePathCalculatorInput input)
        {
            var now = DateTime.Now;
            var year = now.Year.ToString();
            var month = now.Month.ToString().PadLeft(2, '0');
            var day = now.Day.ToString().PadLeft(2, '0');

            var fileName = Path.GetFileNameWithoutExtension(input.FileName);
            var ext = Path.GetExtension(input.FileName);
            var fullFilePath = Path.Combine(input.LocalBase, year, month, day, input.FileName);
            if (!File.Exists(fullFilePath))
            {
                return fullFilePath;
            }

            for (int i = 0; i < int.MaxValue; i++)
            {
                var withPosfixFileName = $"{fileName}.{i + 1}{ext}";
                fullFilePath = Path.Combine(input.LocalBase, year, month, day, withPosfixFileName);
                if (!File.Exists(fullFilePath))
                {
                    return Path.Combine(year, month, day, withPosfixFileName);
                }
            }

            throw new FilePathNotFoundException($"生成路径失败: {input.FileName}");
        }
    }
}
