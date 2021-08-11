using FluentValidation;
using Mediators.Validation;

namespace Mediators
{
    public class CommandValidator : AbstractValidator<Activities.Create>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }
}