using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phonebook.Services.DataCapture.Domain;

namespace Phonebook.Services.DataCapture.Infrastructure.Concrete.EntityFramework
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

        }

        public DbSet<Domain.Phonebook> Phonebooks { get; set; }

       public DbSet<Contact> Contacts { get; set; } 
    }
}
