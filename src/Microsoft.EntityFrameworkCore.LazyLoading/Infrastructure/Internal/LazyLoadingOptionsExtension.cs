using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Metadata.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Query.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Infrastructure.Internal
{
    public class LazyLoadingOptionsExtension : IDbContextOptionsExtension
    {
        public LazyLoadingOptionsExtension()
        {
        }

        // NB: When adding new options, make sure to update the copy ctor below.

        // ReSharper disable once UnusedParameter.Local
        public LazyLoadingOptionsExtension(LazyLoadingOptionsExtension copyFrom)
        {
        }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddScoped<IEntityMaterializerSource, LazyLoadingEntityMaterializerSource>();
            services.AddScoped<EntityFrameworkCore.Internal.IConcurrencyDetector, ConcurrencyDetector>();
            services.AddScoped<ICompiledQueryCache, PerDbContextCompiledQueryCache>();
        }
    }
}
