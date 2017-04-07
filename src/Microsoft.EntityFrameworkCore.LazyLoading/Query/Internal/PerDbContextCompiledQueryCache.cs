using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Query.Internal
{
    public class PerDbContextCompiledQueryCache : CompiledQueryCache
    {
        private readonly ICurrentDbContext _currentDbContext;

        public PerDbContextCompiledQueryCache(IDbContextServices contextServices)
            : base(contextServices)
        {
            _currentDbContext = contextServices.CurrentContext;
        }

        public override Func<QueryContext, IAsyncEnumerable<TResult>> GetOrAddAsyncQuery<TResult>(object cacheKey, Func<Func<QueryContext, IAsyncEnumerable<TResult>>> compiler)
        {
            var cacheKeyWithDbContextId = CreateCacheKeyWithDbContextId(cacheKey);
            return base.GetOrAddAsyncQuery(cacheKeyWithDbContextId, compiler);
        }

        public override Func<QueryContext, TResult> GetOrAddQuery<TResult>(object cacheKey, Func<Func<QueryContext, TResult>> compiler)
        {
            var cacheKeyWithDbContextId = CreateCacheKeyWithDbContextId(cacheKey);
            return base.GetOrAddQuery(cacheKeyWithDbContextId, compiler);
        }

        private object CreateCacheKeyWithDbContextId(object cacheKey)
        {
            return _currentDbContext.Context.GetHashCode() + "__DbContextCompiledQuery__" + cacheKey.GetHashCode();
        }
    }
}
