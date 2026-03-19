using FluentValidation;
using PharmaStock.Core.DTO.Register;
namespace PharmaStock.Core.Validators
{
    public class UpsertUserValidator : AbstractValidator<UpsertUserDTO>
    {
        public UpsertUserValidator()
        {
            RuleFor(x => x.Username)
                .NotNull().WithMessage("Username cannot be null.")
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email cannot be null.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Enter a valid email.");

            RuleFor(x => x.Phone)
                .NotNull().WithMessage("Phone cannot be null.")
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches("^[0-9]{10}$").WithMessage("Phone number must be exactly 10 digits.");

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Please select a valid role.");

            RuleFor(x => x.AdminId)
                .GreaterThan(0).WithMessage("Admin ID must be valid and greater than 0.");
        }
    }
}