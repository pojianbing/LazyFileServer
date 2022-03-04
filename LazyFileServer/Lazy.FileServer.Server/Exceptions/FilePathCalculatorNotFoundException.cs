using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Server.Exceptions
{
    public class FilePathCalculatorNotFoundException : Exception
    {

        public FilePathCalculatorNotFoundException(string message) : base(message)
        {
        }

        public FilePathCalculatorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
