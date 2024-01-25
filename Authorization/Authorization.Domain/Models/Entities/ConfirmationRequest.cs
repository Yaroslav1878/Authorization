using System;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Models.Entities;

public class ConfirmationRequest
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public ConfirmationRequestSubject Subject { get; set; }
    public ConfirmationRequestType ConfirmationType { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public string? Receiver { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public User User { get; set; }

    public int GetRequestHashCode()
    {
        return HashCode.Combine(Id, User.Id);
    }
}
