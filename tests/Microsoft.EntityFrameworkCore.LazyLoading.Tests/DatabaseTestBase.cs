using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Tests.Configuration;
using Microsoft.EntityFrameworkCore.LazyLoading.Tests.Data;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests
{
    public abstract class DatabaseTestBase
    {
        protected enum ContextInitializationOptions
        {
            SeedSampleData,
            CleanupData,
            DoNothing
        }

        private readonly SchoolContextFactory _ctxFactory = new SchoolContextFactory();

        protected SchoolContext CreateDbContext(ConnectionStringSelector connectionStringSelector, ContextInitializationOptions options)
        {
            SchoolContext ctx;
            if (options == ContextInitializationOptions.CleanupData ||
                options == ContextInitializationOptions.SeedSampleData)
            {
                using (ctx = _ctxFactory.Create(new DbContextFactoryOptions(), connectionStringSelector))
                {
                    ctx.Database.EnsureDeleted();
                    if (options == ContextInitializationOptions.SeedSampleData)
                    {
                        DbInitializer.Initialize(ctx);
                    }
                }
            }

            ctx = _ctxFactory.Create(new DbContextFactoryOptions(), connectionStringSelector);
            ctx.Database.EnsureCreated();
            ctx.Database.Migrate();

            return ctx;
        }
    }
}
