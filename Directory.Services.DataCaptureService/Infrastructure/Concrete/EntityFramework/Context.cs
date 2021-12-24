using System.IO;
using Directory.Services.DataCaptureService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Directory.Services.DataCaptureService.Infrastructure.Concrete.EntityFramework
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(System.IO.Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseNpgsql(config["ConnectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Contact>()
            //    .HasOne(c => c.Phonebook)
            //    .WithMany(p => p.Contacts)
            //    .HasForeignKey(p => p.PhonebookId);

            //modelBuilder.Entity<Phonebook>()
            //    .HasMany(p => p.Contacts)
            //    .WithOne();
        }

        public DbSet<Phonebook> Phonebooks { get; set; }

       public DbSet<Contact> Contacts { get; set; } 
    }
}
