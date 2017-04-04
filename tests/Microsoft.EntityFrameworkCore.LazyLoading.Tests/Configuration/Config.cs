using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Configuration
{
    public sealed class Config
    {
        public enum Database
        {
            SqlServer,
            MySql,
            PostgreSql
        }

        public Database DatabaseType { get; }

        public DatabaseConfig SqlServerDatabaseConfig { get; }

        public DatabaseConfig MySqlDatabaseConfig { get; }

        public DatabaseConfig PostgreSqlDatabaseConfig { get; }

        private Config()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables("Microsoft_EntityFrameworkCore_LazyLoading_Tests_")
                .Build();

            SqlServerDatabaseConfig = new DatabaseConfig(config["SqlServer:ConnectionString"]);
            MySqlDatabaseConfig = new DatabaseConfig(config["MySql:ConnectionString"]);
            PostgreSqlDatabaseConfig = new DatabaseConfig(config["PostgreSql:ConnectionString"]);

            if (!Enum.TryParse(config["DatabaseType"], true, out Database parsedDatabase))
            {
                throw new Exception($"Invalid database type (was '{config["DatabaseType"]}')");
            }

            DatabaseType = parsedDatabase;
        }

        private static Config _instance;

        public static Config GetInstance()
        {
            return _instance ?? (_instance = new Config());
        }
    }
}
