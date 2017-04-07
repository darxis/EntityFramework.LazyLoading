using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Exceptions;
using Microsoft.EntityFrameworkCore.LazyLoading.Internal;

namespace Microsoft.EntityFrameworkCore.LazyLoading
{
    public sealed class LazyReference<T>
        where T : class
    {
        private bool _isLoading;

        private bool _isLoaded;

        private T _value;

        private DbContext _ctx;

        public void SetContext(DbContext ctx)
        {
            _ctx = ctx;
        }

        public void SetValue(T value)
        {
            _value = value;
            _isLoaded = true;
        }

        public T GetValue(object parent, string referenceName)
        {
            if (_ctx == null || _isLoaded || _isLoading)
            {
                return _value;
            }

            _isLoading = true;

            try
            {
                var concurrencyDetector =
                    _ctx.GetService<EntityFrameworkCore.Internal.IConcurrencyDetector>() as IConcurrencyDetector;
                if (concurrencyDetector == null)
                {
                    throw new LazyLoadingConfigurationException(
                        $"Service of type '{typeof(EntityFrameworkCore.Internal.IConcurrencyDetector).FullName}' must be replaced by a service of type '{typeof(IConcurrencyDetector).FullName}' in order to use LazyLoading");
                }

                if (concurrencyDetector.IsInOperation())
                {
                    return _value;
                }

                _ctx.Entry(parent).Reference(referenceName).Load();
            }
            finally
            {
                _isLoading = false;
            }

            return _value;
        }
    }
}
