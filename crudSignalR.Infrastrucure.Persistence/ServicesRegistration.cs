using crudSignalR.Core.Application.Interface.Repository;
using crudSignalR.Core.Domain.Entities;
using crudSignalR.Infrastrucure.Persistence.Context;
using crudSignalR.Infrastrucure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Infrastrucure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            services.AddDbContext<ApplicationContext>(options =>
            {
                //options.EnableSensitiveDataLogging();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));
            });
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IStudentRepository,StudentRepository>();
            services.AddTransient<IMessageRepository,MessageRepository>();
            #endregion

        }
    }
}
