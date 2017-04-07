# EntityFramework.LazyLoading [![Build Status](https://travis-ci.org/darxis/EntityFramework.LazyLoading.svg?branch=dev)](https://travis-ci.org/darxis/EntityFramework.LazyLoading)
Lazy Loading for EF Core

Inspired by and partially based on the blog post: https://weblogs.asp.net/ricardoperes/implementing-missing-features-in-entity-framework-core-part-6-lazy-loading

# How to enable LazyLoading in EF Core?

Enabling LazyLoading in EF Core is extremely easy with this library. You just need to call `UseLazyLoading()` (see point 2 below).

However, you will need to slightly modify your entity classes, but just the References, not the Collections (see point 3 below).

## Step 1 - Nuget package
Reference the `Microsoft.EntityFrameworkCore.LazyLoading` NuGet package (https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.LazyLoading/).
## Step 2 - Configure the DbContext builder
Call `UseLazyLoading()` on the `DbContextOptionsBuilder` when creating the `DbContext`.
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
        dbContextOptionsBuilder.UseLazyLoading();
		
        // Build DbContext
        return MyDbContext(dbContextOptionsBuilder.Options);
    }
}
```
## Step 3 - Adjust the model
In your model you need to declare References using the type `LazyReference<T>`. Collections don't require additional configuration in your model, just use the `ICollection<>` type.
```c#
public class Parent
{
    public ICollection<Child> Childs { get; set; }
}

public class Child
{
    private LazyReference<Parent> _parentLazy = new LazyReference<Parent>();
    public Parent Parent
    {
        get
        {
            return _parentLazy.GetValue(this, nameof(Parent));
        }
        set
        {
            _parentLazy.SetValue(value);
        }
    }
}
```
## Step 4 - Done!
That's all, LazyLoading enabled! It was so easy, wasn't it?
