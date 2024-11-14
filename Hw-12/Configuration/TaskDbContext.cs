
using Hw_12.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hw_12.Configuration
{
    public class TaskDbContext : DbContext
    {
        public DbSet<Task> Tasks{ get; set;}
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                (@"Server=DESKTOP-78B19T2\SQLEXPRESS; Initial Catalog=Hwu12; User Id=sa; Password=13771377; TrustServerCertificate=True;");
        }
    }
}
