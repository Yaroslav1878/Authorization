using System.ComponentModel.DataAnnotations;

namespace Authorization.Domain.Enums;

public enum ErrorCode
{
    [Display(Name = "authorizationException")]
    AuthorizationException,
    [Display(Name = "authorizationRefreshException")]
    AuthorizationRefreshException,
    [Display(Name = "authTypeNotFoundException")]
    AuthTypeNotFoundException,
    [Display(Name = "clientAuthorizationException")]
    ClientAuthorizationException,
    [Display(Name = "confirmationRequestExpiredException")]
    ConfirmationRequestExpiredException,
    [Display(Name = "confirmationRequestNotFoundException")]
    ConfirmationRequestNotFoundException,
    [Display(Name = "entityNotFoundException")]
    EntityNotFoundException,
    [Display(Name = "duplicatedEmailException")]
    DuplicatedEmailException,
    [Display(Name = "invalidCredentialsException")]
    InvalidCredentialsException,
    [Display(Name = "missingRefreshTokenException")]
    MissingRefreshTokenException,
    [Display(Name = "roleNotFoundException")]
    RoleNotFoundException,
    [Display(Name = "userAlreadyConfirmedException")]
    UserAlreadyConfirmedException,
    [Display(Name = "userIdNotFoundException")]
    UserIdNotFoundException,
    [Display(Name = "userEmailNotFoundException")]
    UserEmailNotFoundException,
    [Display(Name = "userNotFoundException")]
    UserNotFoundException,
    [Display(Name = "validationFailed")]
    ValidationFailed,
    [Display(Name = "permissionException")]
    PermissionException,
    [Display(Name = "azureAdUnreachableException")]
    AzureAdUnreachableException,
    [Display(Name = "userIsAlreadyActivated")]
    UserIsAlreadyActivated,
    [Display(Name = "BadRequest")]
    BadRequest,
    [Display(Name = "sendgrid_exception")]
    SendGridException,
}