using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Tests
{
    public static class TestNookContextFactory
    {
        public static NookContext Create()
        {
            var options = new DbContextOptionsBuilder<NookContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new NookContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
