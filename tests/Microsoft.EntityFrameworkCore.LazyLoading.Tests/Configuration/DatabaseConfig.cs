using System;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Configuration
{
    public enum ConnectionStringSelector
    {
        Main,
        Second
    }

    public class DatabaseConfig
    {
        public string MainConnectionString { get; }

        public string SecondConnectionString { get; }

        public DatabaseConfig(string mainConnectionString, string secondConnectionString)
        {
            MainConnectionString = mainConnectionString;
            SecondConnectionString = secondConnectionString;
        }

        public string GetConnectionString(ConnectionStringSelector connectionStringSelector)
        {
            switch (connectionStringSelector)
            {
                case ConnectionStringSelector.Main:
                    return MainConnectionString;
                case ConnectionStringSelector.Second:
                    return SecondConnectionString;
                default:
                    throw new Exception($"Unknown {typeof(ConnectionStringSelector)} value (was {connectionStringSelector}).");
            }
        }
    }
}
