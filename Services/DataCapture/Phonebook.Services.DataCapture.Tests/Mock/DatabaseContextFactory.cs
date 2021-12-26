using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Phonebook.Services.DataCapture.Infrastructure.Concrete.EntityFramework;

namespace Phonebook.Services.DataCapture.Tests.Mock
{
    public static class DatabaseContextFactory
    {
        public static Context GetInstance
        {
            get
            {
                var serviceProvider = new ServiceCollection()
                                     .AddEntityFrameworkInMemoryDatabase()
                                     .BuildServiceProvider();

                var options = new DbContextOptionsBuilder<Context>()
                             .UseInMemoryDatabase(Guid.NewGuid().ToString())
                             .UseInternalServiceProvider(serviceProvider)
                             .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                             .EnableSensitiveDataLogging()
                             .Options;

                var db = new Context(options);
                db.Database.EnsureCreated();
                return db;
            }
        }
    }
}