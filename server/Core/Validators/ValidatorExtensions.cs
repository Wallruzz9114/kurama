using FluentValidation;

namespace Core.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> ValidatePassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var ruleBuilderOptions = ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain 1 non-alphanumeric");

            return ruleBuilderOptions;
        }
    }
}