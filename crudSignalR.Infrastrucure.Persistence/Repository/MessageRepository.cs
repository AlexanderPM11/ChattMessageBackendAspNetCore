using crudSignalR.Core.Application.Helper;
using crudSignalR.Core.Application.Interface.Repository;
using crudSignalR.Core.Domain.Entities;
using crudSignalR.Infrastrucure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Infrastrucure.Persistence.Repository
{
    public class MessageRepository:GenericRepository<Message>,IMessageRepository
    {
        private readonly ApplicationContext _dbContext;

        public MessageRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Message> AddImageAsync(Message entity,IFormFile? file)
        {
            if (file != null)
            {
            var resul= await  EncondeImageData.EncondeImage(file);
            entity.FileBytes = resul;
            }
           
            await _dbContext.Set<Message>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DesactiveMessage(Message entity)
        {
            var entry = await _dbContext.Set<Message>().FindAsync(entity.Id);
            _dbContext.Entry(entry).CurrentValues.SetValues(entity);
            int rowsAffected = await _dbContext.SaveChangesAsync();
            // Verificar si la eliminación fue exitosa
            return rowsAffected > 0;
        }

        public async Task<List<Message>> GetAllMessageByUser(string UserFrom, string UserTo)
        {
            var messages= await _dbContext.Set<Message>().ToListAsync();
            var targetMessages = messages.Where(me => (me.From == UserFrom && me.To == UserTo) || (me.From == UserTo && me.To == UserFrom)).OrderByDescending(me=>me.Id)
                .ToList();

            return targetMessages;
        }
        public async Task<List<Message>> GetMessagePagination(int pageNumber, int pageCountRegister, string UserFrom, string UserTo)
        {
            var result = _dbContext.Message
                   .FromSqlRaw("SELECT * FROM dbo.PaginationMessage(@pageNumber, @pageCountRegister, @UserFrom, @UserTo)",
                new SqlParameter("@pageNumber", pageNumber),
                new SqlParameter("@pageCountRegister", pageCountRegister),
                new SqlParameter("@UserFrom", UserFrom),
                new SqlParameter("@UserTo", UserTo))
                   .ToList();

            return result;
        }
       
    }
}
