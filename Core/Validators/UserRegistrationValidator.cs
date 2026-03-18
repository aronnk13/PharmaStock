using FluentValidation;
using PharmaStock.Core.DTO.Register;

namespace PharmaStock.Core.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationDTO>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Enter a valid email.");
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches("^[0-9]{10}$").WithMessage("Phone number must be exactly 10 digits.");
            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                .NotEmpty().WithMessage("User must be assigned a role.");
        }
    }
}