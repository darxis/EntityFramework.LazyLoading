# EntityFramework.LazyLoading
Lazy Loading for EF Core

Inspired by and partially based on the blog post: https://weblogs.asp.net/ricardoperes/implementing-missing-features-in-entity-framework-core-part-6-lazy-loading

# How to enable LazyLoading in EF Core?

1. Reference the `Microsoft.EntityFrameworkCore.LazyLoading` NuGet package.
2. Create (or modify an existing) DbContext factory. Include the lines inside the two `if(_isLazy)` blocks in your DbContext factory (3 lines total - 2 before building the DbContext, and 1 after):
```c#
public class MyDbContextFactory : IDbContextFactory<MyDbContext>
{
    private readonly bool _isLazy;

    public MyDbContextFactory (bool isLazy)
    {
        _isLazy = isLazy;
    }

    public MyDbContext Create(DbContextFactoryOptions options)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
        dbContextOptionsBuilder.UseSqlServer("<some_connection_string>");

        // LazyLoading specific
        if (_isLazy)
        {
            dbContextOptionsBuilder.ReplaceService<Microsoft.EntityFrameworkCore.Metadata.Internal.IEntityMaterializerSource, Microsoft.EntityFrameworkCore.LazyLoading.Internal.LazyLoadingEntityMaterializerSource<MyDbContext>>();
            dbContextOptionsBuilder.ReplaceService<Microsoft.EntityFrameworkCore.Internal.IConcurrencyDetector, Microsoft.EntityFrameworkCore.LazyLoading.Internal.ConcurrencyDetector>();
        }
            

        // Build DbContext
        var ctx = new MyDbContext(dbContextOptionsBuilder.Options);

        // LazyLoading specific
        if (_isLazy)
        {
            (ctx.GetService<Microsoft.EntityFrameworkCore.Metadata.Internal.IEntityMaterializerSource>() as Microsoft.EntityFrameworkCore.LazyLoading.Internal.LazyLoadingEntityMaterializerSource<MyDbContext>).SetDbContext(ctx);
        }

        return ctx;
    }
}
```
3. That's all, LazyLoading enabled.
