using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Dtos.Message
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Messae { get; set; }
        public byte[] FileBytes { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
