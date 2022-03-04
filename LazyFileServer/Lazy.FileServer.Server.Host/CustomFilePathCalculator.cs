using Lazy.FileServer.Server.FilePathCalculators;

namespace Lazy.FileServer.Server.Host.Host
{
    public class CustomFilePathCalculator : IFilePathCalculator
    {
        /// <summary>
        /// 为当前组件起一个名字，以便在配置文件中引用
        /// </summary>
        public string Name
        {
            get
            {
                return "custom";
            }
        }

        /// <summary>
        /// 直接返回文件名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Calculate(FilePathCalculatorInput input)
        {
            return input.FileName;
        }
    }
}
