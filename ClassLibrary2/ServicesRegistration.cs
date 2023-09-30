using crudSegnalR.Infrastructure.httpclient.Http;
using crudSignalR.Core.Application.Interface.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSegnalR.Infrastructure.httpclient
{
    public static class ServicesRegistration
    {
       public static void AddLayerHttp(this IServiceCollection services, IConfiguration configuration)
       {
            services.AddTransient<IGenericHttp,GenericHttp> ();
            services.AddTransient<IFacebookHttp, FacebookHttp>();
       }
    }
}
