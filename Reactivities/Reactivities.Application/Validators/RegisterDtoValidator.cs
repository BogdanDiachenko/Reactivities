using FluentValidation;
using Reactivities.Core.DTOs;
using Reactivities.Core.DTOs.User;

namespace Reactivities.BusinessLogic.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email is not real");
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username can't be empty");
        RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Display name can't be empty");
        RuleFor(x => x.Password).Matches("(?=.*\\d)(?=.*[A-Z]).{8,20}$")
            .WithMessage("Password must have at least 1 digit, uppercase and non alphanumeric");
    }
}