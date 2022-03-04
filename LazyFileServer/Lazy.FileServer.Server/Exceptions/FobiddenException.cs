using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Server.Exceptions
{
    public class FobiddenException : Exception
    {
        public FobiddenException(string message) : base(message)
        {
        }

        public FobiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
