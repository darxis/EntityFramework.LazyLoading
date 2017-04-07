using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Sample.Data.Factory
{
    public class SchoolContextFactory : IDbContextFactory<SchoolContext>
    {
        private readonly bool _isLazy;

        public SchoolContextFactory() : this(false)
        {
        }

        public SchoolContextFactory(bool isLazy)
        {
            _isLazy = isLazy;
        }

        public SchoolContext Create(DbContextFactoryOptions options)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<SchoolContext>();

            dbContextOptionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ContosoUniversity;Trusted_Connection=True;MultipleActiveResultSets=true");

            // LazyLoading specific
            if (_isLazy)
            {
                dbContextOptionsBuilder.UseLazyLoading();
            }

            // Build DbContext
            return new SchoolContext(dbContextOptionsBuilder.Options);
        }
    }
}
