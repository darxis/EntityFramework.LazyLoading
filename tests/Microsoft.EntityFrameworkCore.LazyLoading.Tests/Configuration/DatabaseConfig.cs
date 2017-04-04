namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Configuration
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; }

        public DatabaseConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
