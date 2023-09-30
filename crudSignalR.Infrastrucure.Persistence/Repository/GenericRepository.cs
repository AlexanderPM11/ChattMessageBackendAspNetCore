using crudSignalR.Core.Application.Interface.Repository;
using crudSignalR.Infrastrucure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Infrastrucure.Persistence.Repository
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationContext _applicationContext;

        public GenericRepository(ApplicationContext dbContext)
        {
            _applicationContext = dbContext;
        }

        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            await _applicationContext.Set<Entity>().AddAsync(entity);
            await _applicationContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(Entity entity, int id)
        {
            var entry = await _applicationContext.Set<Entity>().FindAsync(id);
            _applicationContext.Entry(entry).CurrentValues.SetValues(entity);
            await _applicationContext.SaveChangesAsync();
        }
        public virtual async Task<bool> DeleteAsync(Entity entity)
        {
            _applicationContext.Set<Entity>().Remove(entity);
            int rowsAffected = await _applicationContext.SaveChangesAsync();
            // Verificar si la eliminación fue exitosa
            return rowsAffected > 0;
        }

        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return await _applicationContext.Set<Entity>().ToListAsync();//Deferred execution
        }


        public virtual async Task<Entity> GetByIdAsync(int id)
        {
            return await _applicationContext.Set<Entity>().FindAsync(id);
        }

    }
}
