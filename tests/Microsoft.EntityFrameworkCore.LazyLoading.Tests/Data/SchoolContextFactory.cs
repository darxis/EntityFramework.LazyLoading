using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Metadata.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Query.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Tests.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
            dbContextOptionsBuilder.ReplaceService<IEntityMaterializerSource, LazyLoadingEntityMaterializerSource<SchoolContext>>();
            dbContextOptionsBuilder.ReplaceService<EntityFrameworkCore.Internal.IConcurrencyDetector, ConcurrencyDetector>();
            dbContextOptionsBuilder.ReplaceService<ICompiledQueryCache, PerDbContextCompiledQueryCache>();

            var ctx = new SchoolContext(dbContextOptionsBuilder.Options);

            // LazyLoading specific
            // ReSharper disable PossibleNullReferenceException
            (ctx.GetService<IEntityMaterializerSource>() as LazyLoadingEntityMaterializerSource<SchoolContext>).SetDbContext(ctx);
            // ReSharper restore PossibleNullReferenceException

            ctx.Database.EnsureCreated();
            ctx.Database.Migrate();

            return ctx;
        }
    }
}
