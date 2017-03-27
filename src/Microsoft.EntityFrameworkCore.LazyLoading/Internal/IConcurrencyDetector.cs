namespace Microsoft.EntityFrameworkCore.LazyLoading.Internal
{
    public interface IConcurrencyDetector : EntityFrameworkCore.Internal.IConcurrencyDetector
    {
        bool IsInOperation();
    }
}
