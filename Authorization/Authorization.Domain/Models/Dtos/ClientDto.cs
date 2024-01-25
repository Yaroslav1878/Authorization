using System;

namespace Authorization.Domain.Models.Dtos
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
