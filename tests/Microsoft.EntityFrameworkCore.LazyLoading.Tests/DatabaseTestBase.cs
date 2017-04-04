using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Tests.Data;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests
{
    public abstract class DatabaseTestBase
    {
        private readonly SchoolContextFactory _ctxFactory = new SchoolContextFactory();

        protected SchoolContext CreateDbContext()
        {
            using (var ctx = _ctxFactory.Create(new DbContextFactoryOptions()))
            {
                DbInitializer.Initialize(ctx);
            }

            return _ctxFactory.Create(new DbContextFactoryOptions());
        }
    }
}
