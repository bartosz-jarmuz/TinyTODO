using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Persistence
{
    public sealed class ToDoLiteDbContext : DbContext 
    {
#pragma warning disable CS8618
        public ToDoLiteDbContext()
        {
            Database.Migrate();
        }

        public ToDoLiteDbContext(DbContextOptions<ToDoLiteDbContext> options) : base(options) { }
#pragma warning restore CS8618
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=ToDoLite.db", options =>
                {
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().Property(p => p.Name).UseCollation("NOCASE"); //tags should be case insensitive
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
