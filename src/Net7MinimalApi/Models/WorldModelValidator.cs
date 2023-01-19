using FluentValidation;

namespace Net7MinimalApi.Models
{
    public class WorldModelValidator: AbstractValidator<WorldModel>
    {
        public WorldModelValidator()
        {
            RuleFor(_ => _.Id)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("The field Id must be between 1 and 2147483647.");

            RuleFor(_ => _.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("The Name field is required.")
                .Matches("^[a-zA-Z_-]+$");
        }
    }
}
