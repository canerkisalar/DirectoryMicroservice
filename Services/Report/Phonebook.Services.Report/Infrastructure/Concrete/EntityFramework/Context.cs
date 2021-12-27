using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phonebook.Services.Report.Models;
using Phonebook.Services.Report.Models.Report;

namespace Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework
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
           // client.Database.EnsureCreated();
        }
        public Context()
        {
        }

        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Phonebook> Phonebooks { get; set; }
       public DbSet<Contact> Contacts { get; set; }
       public DbSet<ReportHead> ReportHead { get; set; }
       public DbSet<ReportItem> ReportItem { get; set; }
    }
}
