using crudSignalR.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Infrastrucure.Persistence.Context
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Student> Student { get; set; }
        public DbSet<Message> Message { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tables 
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Message>().ToTable("Message");
            #endregion
            #region Primary Keys
            modelBuilder.Entity<Student>().HasKey(std => std.id);
            modelBuilder.Entity<Message>().HasKey(std => std.Id);
            #endregion
        }

    }
}
