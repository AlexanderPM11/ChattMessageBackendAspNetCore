using crudSignalR.Core.Application.Interface.Repository;
using crudSignalR.Core.Domain.Entities;
using crudSignalR.Infrastrucure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Infrastrucure.Persistence.Repository
{
    public class StudentRepository:GenericRepository<Student>,IStudentRepository
    {
        private readonly ApplicationContext _dbContext;

        public StudentRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
