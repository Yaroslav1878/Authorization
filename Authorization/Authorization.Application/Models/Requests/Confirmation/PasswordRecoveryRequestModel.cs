using System.ComponentModel.DataAnnotations;
using Authorization.Application.Helpers;

namespace Authorization.Application.Models.Requests.Confirmation;

public class PasswordRecoveryRequestModel
{
    [Required]
    [EmailAddress]
    [RegularExpression(
        RegexHelper.EnglishLettersNumbersAndEmailCharacters,
        ErrorMessage = $"{nameof(Email)} must contain only letters, numbers, dot and @ character.")]
    public string Email { get; set; }
}
