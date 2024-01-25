using System.ComponentModel.DataAnnotations;
using Authorization.Application.Helpers;

namespace Authorization.Application.Models.Requests.UserManagement;

public class RegisterUserRequestModel
{
    [Required]
    [EmailAddress]
    [RegularExpression(
        RegexHelper.EnglishLettersNumbersAndEmailCharacters,
        ErrorMessage = $"{nameof(Email)} must contain only letters, numbers, dot and @ character.")]
    public string Email { get; set; }

    [Required, RegularExpression(
         RegexHelper.EnglishLetters,
         ErrorMessage = $"{nameof(FirstName)} can only contain letters.")]
    public string FirstName { get; set; }

    [Required, RegularExpression(
         RegexHelper.EnglishLetters,
         ErrorMessage = $"{nameof(LastName)} can only contain letters.")]
    public string LastName { get; set; }
    
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    [RegularExpression(
        RegexHelper.PasswordCharacters,
        ErrorMessage = "Password must contain only letters, numbers, special characters and no spaces.")]
    public string Password { get; set; }
}
