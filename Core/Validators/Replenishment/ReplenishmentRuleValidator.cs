using FluentValidation;
using PharmaStock.Core.DTO.Replenishment;

namespace PharmaStock.Core.Validators.Replenishment
{
    public class ReplenishmentRuleValidator : AbstractValidator<UpsertReplenishmentRuleDTO>
    {
        public ReplenishmentRuleValidator()
        {
            RuleFor(x => x.LocationId)
                .GreaterThan(0)
                .WithMessage("LocationId must be greater than 0.");

            RuleFor(x => x.ItemId)
                .GreaterThan(0)
                .WithMessage("ItemId must be greater than 0.");

            RuleFor(x => x.MaxLevel)
                .GreaterThan(0)
                .WithMessage("MaxLevel must be greater than 0.");
                
            RuleFor(x => x.MinLevel)
                .GreaterThan(0).WithMessage("MinLevel must be greater than 0.")
                .LessThan(x => x.ParLevel).WithMessage("MinLevel must be less than ParLevel.");

            RuleFor(x => x.ParLevel)
                .GreaterThan(x => x.MinLevel).WithMessage("ParLevel must be greater than MinLevel.")
                .LessThan(x => x.MaxLevel).WithMessage("ParLevel must be less than MaxLevel.");

            RuleFor(x => x.ReviewCycle);
        }
    }
}