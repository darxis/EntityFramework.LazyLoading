namespace Microsoft.EntityFrameworkCore.LazyLoading.Internal
{
    public interface ILazyLoadingEntityMaterializerSource<TDbContext>
        where TDbContext : DbContext
    {
        void SetDbContext(TDbContext ctx);
    }
}
