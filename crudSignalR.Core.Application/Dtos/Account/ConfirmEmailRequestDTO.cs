using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Dtos.Account
{
    public class ConfirmEmailRequestDTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
