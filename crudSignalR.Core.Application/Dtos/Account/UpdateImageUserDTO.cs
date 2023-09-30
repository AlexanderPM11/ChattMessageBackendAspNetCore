using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace crudSignalR.Core.Application.Dtos.Account
{
    public class UpdateImageUserDTO
    {
        public string userId { get; set; }
        public IFormFile file { get; set; }


    }
}
