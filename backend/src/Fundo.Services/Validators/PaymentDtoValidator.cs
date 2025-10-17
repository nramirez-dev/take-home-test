using FluentValidation;
using Fundo.Services.DTOs;

namespace Fundo.Services.Validators;

public class PaymentDtoValidator : AbstractValidator<PaymentDto>
{
    public PaymentDtoValidator()
    {
        RuleFor(x => x.PaymentAmount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than zero.");
    }
}