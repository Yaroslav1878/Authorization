using Authorization.Domain.Configurations.Abstractions;

namespace Authorization.Domain.Configurations
{
    public class PersistenceConfiguration : IPersistenceConfiguration
    {
        public string ConnectionString { get; set; }

        public string Schema { get; set; }
    }
}
