namespace Microsoft.EntityFrameworkCore.LazyLoading.Sample.Data.Factory
{
    public class LazySchoolContextFactory : SchoolContextFactoryBase
    {
        public LazySchoolContextFactory() : base(true)
        {
        }
    }
}
