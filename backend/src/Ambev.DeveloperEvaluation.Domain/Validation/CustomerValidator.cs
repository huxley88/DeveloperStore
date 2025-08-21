using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Customer name is required.")
            .MinimumLength(3).WithMessage("Customer name must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

        RuleFor(c => c.Email)
            .SetValidator(new EmailValidator());

        RuleFor(c => c.Phone)
            .Matches(@"^\+[1-9]\d{10,14}$")
            .When(c => !string.IsNullOrEmpty(c.Phone))
            .WithMessage("Phone number must start with '+' followed by 11-15 digits.");
    }
}