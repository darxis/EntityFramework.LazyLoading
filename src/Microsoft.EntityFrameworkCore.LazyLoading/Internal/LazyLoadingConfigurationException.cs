using System;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Internal
{
    public class LazyLoadingConfigurationException : Exception
    {
        public LazyLoadingConfigurationException(string message) : base(message)
        {
        }
    }
}
