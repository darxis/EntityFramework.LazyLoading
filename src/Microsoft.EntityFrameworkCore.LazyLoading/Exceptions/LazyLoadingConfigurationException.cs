using System;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Exceptions
{
    public class LazyLoadingConfigurationException : Exception
    {
        public LazyLoadingConfigurationException(string message) : base(message)
        {
        }
    }
}
