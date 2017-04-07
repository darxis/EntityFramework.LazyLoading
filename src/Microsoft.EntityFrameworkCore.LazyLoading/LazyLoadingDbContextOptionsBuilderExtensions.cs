using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Infrastructure.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class LazyLoadingDbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseLazyLoading(this DbContextOptionsBuilder optionsBuilder)
        {
            var extension = GetOrCreateExtension(optionsBuilder);
            ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }

        public static DbContextOptionsBuilder<TContext> UseLazyLoading<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>) UseLazyLoading((DbContextOptionsBuilder) optionsBuilder);

        private static LazyLoadingOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
        {
            var existing = optionsBuilder.Options.FindExtension<LazyLoadingOptionsExtension>();
            return existing != null
                ? new LazyLoadingOptionsExtension(existing)
                : new LazyLoadingOptionsExtension();
        }
    }
}
