using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Domain.Entities
{
    public class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public byte age { get; set; }
    }
}
