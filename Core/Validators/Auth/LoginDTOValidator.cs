using FluentValidation;
using PharmaStock.Core.DTO.Auth;

namespace PharmaStock.Core.Validators.Auth
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            Console.WriteLine("LoginDTO Validator is Registered and Running");
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
