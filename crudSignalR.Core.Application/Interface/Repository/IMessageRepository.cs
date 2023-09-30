using crudSignalR.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Interface.Repository
{
    public interface IMessageRepository: IGenericRepository<Message>
    {
        Task<List<Message>> GetAllMessageByUser(string UserFrom, string UserTo);
        Task<List<Message>> GetMessagePagination(int pageNumber, int pageCountRegister, string UserFrom, string UserTo);
        Task<Message> AddImageAsync(Message entity, IFormFile file);
        Task<bool> DesactiveMessage(Message entity);
    }
}
