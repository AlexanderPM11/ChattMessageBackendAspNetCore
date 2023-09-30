using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Dtos.ExternalLogin.Facabook
{
    public partial class UserValidFacebookDto
    {
        public DataUserValid Data { get; set; }
    }

    public class DataUserValid
    {
        public string? App_Id { get; set; }
        public string? Type { get; set; }
        public string? Application { get; set; }
        public long Data_Access_Expires_At { get; set; }
        public long Expires_At { get; set; }
        public bool Is_Valid { get; set; }
        public string? User_Id { get; set; }
    }
}
