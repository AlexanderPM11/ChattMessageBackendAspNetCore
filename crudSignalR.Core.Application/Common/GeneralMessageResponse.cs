using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Common
{
    public static class GeneralMessageResponse
    {
        public static string NotFoundMesage = "BadRequest... Usuario no encontrado";
        public static string Error = "Ocurred error";
        public static string Success = "Registro creado con éxito";
    }
}
