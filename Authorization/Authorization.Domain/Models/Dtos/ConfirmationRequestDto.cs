using System;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Models.Dtos;

public class ConfirmationRequestDto
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
    public UserDto User { get; set; }
}
