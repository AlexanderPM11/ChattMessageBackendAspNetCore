using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Dtos.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public bool IsOnline { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JWToken { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public byte[]? PictureBytes { get; set; }
    }
}
