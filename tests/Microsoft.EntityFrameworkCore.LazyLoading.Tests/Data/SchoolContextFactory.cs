using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Tests.Configuration;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Data
{
    public class SchoolContextFactory : IDbContextFactory<SchoolContext>
    {
        public SchoolContext Create(DbContextFactoryOptions options)
        {
            return Create(options, ConnectionStringSelector.Main);
        }

        public SchoolContext Create(DbContextFactoryOptions options, ConnectionStringSelector connectionStringSelector)
        {
            var config = Config.GetInstance();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<SchoolContext>();

            switch (config.DatabaseType)
            {
                case Config.Database.MySql:
                    dbContextOptionsBuilder
                        .UseMySQL(config.MySqlDatabaseConfig.GetConnectionString(connectionStringSelector));
                    break;
                case Config.Database.SqlServer:
                    dbContextOptionsBuilder
                        .UseSqlServer(config.SqlServerDatabaseConfig.GetConnectionString(connectionStringSelector));
                    break;
                case Config.Database.PostgreSql:
                    dbContextOptionsBuilder
                        .UseNpgsql(config.PostgreSqlDatabaseConfig.GetConnectionString(connectionStringSelector));
                    break;
                default:
                    throw new Exception($"Unknown database type (was '{config.DatabaseType}')");
            }

            dbContextOptionsBuilder.UseLazyLoading();

            var ctx = new SchoolContext(dbContextOptionsBuilder.Options);

            return ctx;
        }
    }
}
