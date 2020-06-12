using FluentValidation;
using Tweetbook.Contracts.V1.Request;

namespace Tweetbook.Validators
{
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        public CreateTagRequestValidator()
        {
            RuleFor(x => x.TagName)
                .NotEmpty()
                .Matches("^[a-zA Z0-9 ]*$");
        }
    }
}
