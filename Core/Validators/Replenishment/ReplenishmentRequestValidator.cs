using FluentValidation;
using PharmaStock.Core.DTO.Replenishment;
using PharmaStock.Core.Interfaces.Repository;

namespace PharmaStock.Core.Validators.Replenishment
{
    public class ReplenishmentRequestValidator : AbstractValidator<UpsertReplenishmentRequestDTO>
    {
        public ReplenishmentRequestValidator()
        {
            RuleFor(x => x.LocationId)
                .GreaterThan(0)
                .WithMessage("LocationId must be greater than 0.");

            RuleFor(x => x.ItemId)
                .GreaterThan(0)
                .WithMessage("ItemId must be greater than 0.");

            RuleFor(x => x.LocationType)
                .IsInEnum()
                .WithMessage("Invalid LocationType.");

            RuleFor(x => x.RuleId)
                .GreaterThan(0)
                .WithMessage("RuleId must be greater than 0.");
                
            RuleFor(x => x.SuggestedQuantity).
                GreaterThan(0)
                .WithMessage("Suggested Quantity must be greater than 0.");
        }
    }
}