using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Dtos.ExternalLogin.Facabook
{
    public class UserDataFacebookDTO
    {
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Email { get; set; }
        public Picture Picture  { get; set; }
    }
    public partial class Picture
    {
        public Data Data { get; set; }
    }

    public partial class Data
    {
        public long Height { get; set; }
        public bool IsSilhouette { get; set; }
        public Uri Url { get; set; }
        public long Width { get; set; }
    }

}
