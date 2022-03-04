using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Server.FilePathCalculators
{
    public interface IFilePathCalculator
    {
        /// <summary>
        /// 计算路径
        /// 返回app之后的路径
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Calculate(FilePathCalculatorInput input);
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get;}
    }
}
