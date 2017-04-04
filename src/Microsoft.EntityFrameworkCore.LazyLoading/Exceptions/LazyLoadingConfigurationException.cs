using System;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Internal.Exceptions
{
    public class LazyLoadingConfigurationException : Exception
    {
        public LazyLoadingConfigurationException(string message) : base(message)
        {
        }
    }
}
