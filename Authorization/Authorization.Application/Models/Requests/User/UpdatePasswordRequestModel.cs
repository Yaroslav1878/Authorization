using System.ComponentModel.DataAnnotations;
using Authorization.Application.Helpers;
using FluentValidation;

namespace Authorization.Application.Models.Requests.User;

public class UpdatePasswordRequestModel
{
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    [RegularExpression(
        RegexHelper.PasswordCharacters,
        ErrorMessage = "Password must contain only letters, numbers, special characters and no spaces.")]
    public string NewPassword { get; set; }

    [Required]
    public string NewPasswordConfirmation { get; set; }
}

public class UpdatePasswordRequestModelValidator : AbstractValidator<UpdatePasswordRequestModel>
{
    public UpdatePasswordRequestModelValidator()
    {
        RuleFor(customer => customer.NewPasswordConfirmation).Equal(customer => customer.NewPassword);
    }
}
