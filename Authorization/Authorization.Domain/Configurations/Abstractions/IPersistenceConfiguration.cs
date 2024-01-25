namespace Authorization.Domain.Configurations.Abstractions
{
    public interface IPersistenceConfiguration
    {
        public string ConnectionString { get; }
        public string Schema { get; }
    }
}
