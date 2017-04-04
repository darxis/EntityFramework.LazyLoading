using System.IO;
using Microsoft.Extensions.Configuration;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Configuration
{
    public sealed class Config
    {
        public DatabaseConfig SqlServerDatabaseConfig { get; }

        private Config()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            SqlServerDatabaseConfig = new DatabaseConfig(config["SqlServer:ConnectionString"]);
        }

        private static Config _instance;

        public static Config GetInstance()
        {
            return _instance ?? (_instance = new Config());
        }
    }
}
