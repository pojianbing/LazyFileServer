using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.FileServer.Client
{
    public class FileServerResponse
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public List<string> Result { get; set; }
    }
}
