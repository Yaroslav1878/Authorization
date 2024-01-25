using System.ComponentModel.DataAnnotations;
using Authorization.Application.Helpers;

namespace Authorization.Application.Models.Requests.Confirmation;

public class PasswordConfirmationRequestModel
{
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    [RegularExpression(
        RegexHelper.PasswordCharacters,
        ErrorMessage = "Password must contain only letters, numbers, special characters and no spaces.")]
    public string NewPassword { get; set; }
}