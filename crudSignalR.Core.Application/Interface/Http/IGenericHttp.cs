using crudSignalR.Core.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Interface.Http
{
    public interface IGenericHttp
    {
        Task<GenericApiResponse<string>> Post(string requestUri, dynamic jsonData);
        Task<GenericApiResponse<string>> Get(string requestUri);
    }
}
