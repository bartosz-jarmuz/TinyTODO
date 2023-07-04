using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Persistence
{
    public class ToDoLiteDbContext : DbContext 
    {
#pragma warning disable CS8618
        public ToDoLiteDbContext()
#pragma warning restore CS8618
        {
            Database.Migrate();
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
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
            modelBuilder.Entity<ToDoItem>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.ToDoItems);

            base.OnModelCreating(modelBuilder);
        }

        public async Task RecreateDatabaseAsync()
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();
        }

    }


}
