using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace crudSignalR.Core.Application.Helper
{
    public class EncondeImageData
    {
        public static async Task<byte[]> EncondeImage(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var resultBytes = memoryStream.ToArray();

            return resultBytes;            
        }
    }
}
