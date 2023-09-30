using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection servides)
        {
            servides.AddAutoMapper(Assembly.GetExecutingAssembly());
            servides.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
