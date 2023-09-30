using crudSignalR.Core.Application.Dtos.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Interface.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest emailRequest); 
    }
}
