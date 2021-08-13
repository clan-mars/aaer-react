using DTO;
using FluentValidation;

namespace Mediators
{
    public class Commentvalidator : AbstractValidator<CommentDto>
    {
        public Commentvalidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}