using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyReadingNookApp.Models
{
    public class NookContextFactory : IDesignTimeDbContextFactory<NookContext>
    {
        public NookContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NookContext>();

            string connectionString =
                "server=localhost;Port=3306;Database=MyReadingNookDB;Uid=root;Pwd=Josh2004*;";

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 46));

            optionsBuilder.UseMySql(connectionString, serverVersion);

            return new NookContext(optionsBuilder.Options);
        }
    }
}
