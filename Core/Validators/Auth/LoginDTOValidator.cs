using FluentValidation;
using PharmaStock.Core.DTO.Auth;

namespace PharmaStock.Core.Validators.Auth
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required")
                .NotNull().WithMessage("Username cannot be Null !");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required")
                .NotNull().WithMessage("Password cannot be Null !");
        }
    }
}
