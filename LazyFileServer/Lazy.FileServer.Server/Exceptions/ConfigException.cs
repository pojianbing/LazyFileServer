using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Server.Exceptions
{
    public class ConfigException : Exception
    {
        public ConfigException(string message) : base(message)
        {
        }

        public ConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
