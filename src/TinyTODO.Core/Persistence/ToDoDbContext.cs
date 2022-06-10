using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TinyTODO.Core.DataModel;

namespace TinyTODO.Core
{
    public class ToDoDbContext : DbContext 
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=TinyTODO.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }


}
