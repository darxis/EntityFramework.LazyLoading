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
            var config = Config.GetInstance();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<SchoolContext>();

            switch (config.DatabaseType)
            {
                case Config.Database.MySql:
                    dbContextOptionsBuilder
                        .UseMySQL(config.MySqlDatabaseConfig.ConnectionString);
                    break;
                case Config.Database.SqlServer:
                    dbContextOptionsBuilder
                        .UseSqlServer(config.SqlServerDatabaseConfig.ConnectionString);
                    break;
                case Config.Database.PostgreSql:
                    dbContextOptionsBuilder
                        .UseNpgsql(config.PostgreSqlDatabaseConfig.ConnectionString);
                    break;
                default:
                    throw new Exception($"Unknown database type (was '{config.DatabaseType}')");
            }

            // LazyLoading specific
            dbContextOptionsBuilder.UseLazyLoading();

            var ctx = new SchoolContext(dbContextOptionsBuilder.Options);

            ctx.Database.EnsureCreated();
            ctx.Database.Migrate();

            return ctx;
        }
    }
}
