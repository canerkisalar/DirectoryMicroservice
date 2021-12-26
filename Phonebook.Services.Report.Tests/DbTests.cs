
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework;
using Phonebook.Services.Report.Tests.Mock;

namespace Phonebook.Services.Report.Tests
{
    public class DbTests
    {
        private static Context _db;

        [SetUp]
        public void Setup()
        {
            _db = DatabaseContextFactory.GetInstance;

        }


        [Test]
        public void TestDbMigrations()
        {

            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

        }
    }
}
