namespace Microsoft.EntityFrameworkCore.LazyLoading.Metadata.Internal
{
    public interface ILazyLoadingEntityMaterializerSource<in TDbContext>
        where TDbContext : DbContext
    {
        void SetDbContext(TDbContext ctx);
    }
}
