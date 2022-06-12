using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using TinyTODO.Core.DataModel;

namespace TinyTODO.Core
{
    public class TinyToDoDbContext : DbContext 
    {
        public TinyToDoDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Setting> Settings { get; set; }

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
            modelBuilder.Entity<Setting>().HasKey(s => s.Key);
            base.OnModelCreating(modelBuilder);
        }

    }


}
