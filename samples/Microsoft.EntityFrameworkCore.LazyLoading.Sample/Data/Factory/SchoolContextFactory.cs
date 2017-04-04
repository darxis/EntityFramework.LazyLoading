using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Metadata.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
                dbContextOptionsBuilder.ReplaceService<IEntityMaterializerSource, LazyLoadingEntityMaterializerSource<SchoolContext>>();
                dbContextOptionsBuilder.ReplaceService<EntityFrameworkCore.Internal.IConcurrencyDetector, ConcurrencyDetector>();
                dbContextOptionsBuilder.ReplaceService<ICompiledQueryCache, PerDbContextCompiledQueryCache>();
            }
            

            // Build DbContext
            var ctx = new SchoolContext(dbContextOptionsBuilder.Options);

            // LazyLoading specific
            if (_isLazy)
            {
                (ctx.GetService<IEntityMaterializerSource>() as LazyLoadingEntityMaterializerSource<SchoolContext>).SetDbContext(ctx);
                (ctx.GetService<ICompiledQueryCache>() as PerDbContextCompiledQueryCache).SetDbContext(ctx);
            }

            return ctx;
        }
    }
}
