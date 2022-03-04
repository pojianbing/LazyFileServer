using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class FormFileExtensions
    {
        public static byte[] GetAllBytes(this IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                return stream.GetAllBytes();
            }
        }

        public static async Task<byte[]> GetAllBytesAsync(this IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                return await stream.GetAllBytesAsync();
            }
        }
    }
}
