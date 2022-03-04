using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Server.Exceptions
{
    public class FilePathNotFoundException : Exception
    {
        public FilePathNotFoundException(string message) : base(message)
        {
        }

        public FilePathNotFoundException(string message, Exception innerException): base(message, innerException)
        {
        }

    }
}
