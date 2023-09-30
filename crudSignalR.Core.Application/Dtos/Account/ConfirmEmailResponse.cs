using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Dtos.Account
{
    public class ConfirmEmailResponse
    {
        public string Message { get; set; }
        public bool Result { get; set; }
    }
}
