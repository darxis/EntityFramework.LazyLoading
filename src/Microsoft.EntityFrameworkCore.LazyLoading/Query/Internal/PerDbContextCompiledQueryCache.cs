using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.LazyLoading.Exceptions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Query.Internal
{
    public class PerDbContextCompiledQueryCache : CompiledQueryCache
    {
        private ICurrentDbContext _dbContext;

        public PerDbContextCompiledQueryCache(IDbContextServices contextServices)
            : base(contextServices)
        {
            _dbContext = contextServices.CurrentContext;
        }

        public override Func<QueryContext, IAsyncEnumerable<TResult>> GetOrAddAsyncQuery<TResult>(object cacheKey, Func<Func<QueryContext, IAsyncEnumerable<TResult>>> compiler)
        {
            if (_dbContext == null)
            {
                throw new LazyLoadingConfigurationException("SetDbContext must be called prior to making queries.");
            }

            var cacheKeyWithDbContextId = CreateCacheKeyWithDbContextId(cacheKey);
            return base.GetOrAddAsyncQuery(cacheKeyWithDbContextId, compiler);
        }

        public override Func<QueryContext, TResult> GetOrAddQuery<TResult>(object cacheKey, Func<Func<QueryContext, TResult>> compiler)
        {
            if (_dbContext == null)
            {
                throw new LazyLoadingConfigurationException("SetDbContext must be called prior to making queries.");
            }

            var cacheKeyWithDbContextId = CreateCacheKeyWithDbContextId(cacheKey);
            return base.GetOrAddQuery(cacheKeyWithDbContextId, compiler);
        }

        private object CreateCacheKeyWithDbContextId(object cacheKey)
        {
            return _dbContext.GetHashCode() + "__DbContextCompiledQuery__" + cacheKey.GetHashCode();
        }
    }
}
