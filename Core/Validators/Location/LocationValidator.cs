using FluentValidation;
using PharmaStock.Core.DTO.Location;

namespace PharmaStock.Core.Validators.Location
{
    public class CreateLocationValidator : AbstractValidator<CreateLocationDTO>
    {
        public CreateLocationValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Location name cannot be null.")
                .NotEmpty().WithMessage("Location name is required.")
                .MaximumLength(255).WithMessage("Location name cannot exceed 255 characters.");

            RuleFor(x => x.LocationTypeId)
                .GreaterThan(0).WithMessage("Location type must be a valid ID greater than 0.");

            RuleFor(x => x.ParentLocationId)
                .GreaterThan(0).WithMessage("Parent location ID must be greater than 0 if provided.")
                .When(x => x.ParentLocationId.HasValue);
        }
    }

    public class UpdateLocationValidator : AbstractValidator<UpdateLocationDTO>
    {
        public UpdateLocationValidator()
        {
            RuleFor(x => x.LocationId)
                .GreaterThan(0).WithMessage("Location ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Location name cannot be null.")
                .NotEmpty().WithMessage("Location name is required.")
                .MaximumLength(255).WithMessage("Location name cannot exceed 255 characters.");

            RuleFor(x => x.LocationTypeId)
                .GreaterThan(0).WithMessage("Location type must be a valid ID greater than 0.");

            RuleFor(x => x.ParentLocationId)
                .GreaterThan(0).WithMessage("Parent location ID must be greater than 0 if provided.")
                .When(x => x.ParentLocationId.HasValue);
        }
    }
}
