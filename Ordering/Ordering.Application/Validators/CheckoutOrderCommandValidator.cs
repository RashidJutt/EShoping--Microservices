using FluentValidation;
using Ordering.Application.Commnads;

namespace Ordering.Application.Validators;

public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
    {
        RuleFor(x => x.UserName)
              .NotEmpty()
              .WithMessage("{UserName} is required")
              .NotNull()
              .MaximumLength(70)
              .WithMessage("{UserName} must not be greater then 70 characters");

        RuleFor(x => x.TotalPrice)
            .NotEmpty()
            .WithMessage("{TotalPrice} is required")
            .GreaterThan(-1)
            .WithMessage("{TotalPrice} must not be -ve");

        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .WithMessage("{EmailAddress} is required");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("{EmailAddress} is required");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("{EmailAddress} is required");
    }
}
