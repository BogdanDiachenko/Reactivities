using FluentValidation;
using Reactivities.Core.DTOs;
using Reactivities.Core.DTOs.User;

namespace Reactivities.BusinessLogic.Validators;

public class LoginDtoValidator: AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
}