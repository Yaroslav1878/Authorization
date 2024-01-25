using System;

namespace Authorization.Domain.Models.Entities;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
