using Authorization.Domain.Configurations.Abstractions;

namespace Authorization.Domain.Configurations
{
    public class FileServiceConfiguration : IFileServiceConfiguration
    {
        public string FileServiceUrl { get; set; }
    }
}