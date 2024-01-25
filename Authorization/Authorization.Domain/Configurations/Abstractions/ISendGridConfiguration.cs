namespace Authorization.Domain.Configurations.Abstractions
{
    public interface ISendGridConfiguration
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string ApiKey { get; set; }
    }
}
