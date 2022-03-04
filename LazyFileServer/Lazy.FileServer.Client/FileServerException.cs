using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Client
{
    public class FileServerException : Exception
    {
        public FileServerException(string message) : base(message)
        {
        }

        public FileServerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
