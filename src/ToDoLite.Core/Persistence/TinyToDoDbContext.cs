using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core
{
    public class ToDoLiteDbContext : DbContext 
    {
        public ToDoLiteDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=ToDoLite.db", options =>
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
