using FluentValidation;
using PharmaStock.Core.DTO.Location;
using PharmaStock.Core.Interfaces.Repository;

namespace PharmaStock.Core.Validators.Location
{
    public class CreateLocationValidator : AbstractValidator<CreateLocationDTO>
    {
        public CreateLocationValidator(ILocationRepository locationRepository)
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Location name cannot be null.")
                .NotEmpty().WithMessage("Location name is required.")
                .MaximumLength(255).WithMessage("Location name cannot exceed 255 characters.")
                .MustAsync(async (dto, name, cancellation) =>
                    !await locationRepository.IsLocationExists(name, dto.LocationTypeId))
                .WithMessage("A location with this name and type already exists.");

            RuleFor(x => x.LocationTypeId)
                .GreaterThan(0).WithMessage("Location type must be a valid ID greater than 0.");

            RuleFor(x => x.ParentLocationId)
                .GreaterThan(0).WithMessage("Parent location ID must be greater than 0 if provided.")
                .When(x => x.ParentLocationId.HasValue);
        }
    }

    public class UpdateLocationValidator : AbstractValidator<UpdateLocationDTO>
    {
        public UpdateLocationValidator(ILocationRepository locationRepository)
        {
            RuleFor(x => x.LocationId)
                .GreaterThan(0).WithMessage("Location ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Location name cannot be null.")
                .NotEmpty().WithMessage("Location name is required.")
                .MaximumLength(255).WithMessage("Location name cannot exceed 255 characters.")
                .MustAsync(async (dto, name, cancellation) =>
                    !await locationRepository.IsLocationExists(name, dto.LocationTypeId, dto.LocationId))
                .WithMessage("A location with this name and type already exists.");

            RuleFor(x => x.LocationTypeId)
                .GreaterThan(0).WithMessage("Location type must be a valid ID greater than 0.");

            RuleFor(x => x.ParentLocationId)
                .GreaterThan(0).WithMessage("Parent location ID must be greater than 0 if provided.")
                .When(x => x.ParentLocationId.HasValue);
        }
    }
}
