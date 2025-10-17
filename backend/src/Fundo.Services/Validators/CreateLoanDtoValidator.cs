using FluentValidation;
using Fundo.Services.DTOs;

namespace Fundo.Services.Validators;

public class CreateLoanDtoValidator : AbstractValidator<CreateLoanDto>
{
    public CreateLoanDtoValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Loan amount must be greater than zero.");

        RuleFor(x => x.ApplicantName)
            .NotEmpty().WithMessage("Applicant name is required.")
            .MaximumLength(100).WithMessage("Applicant name cannot exceed 100 characters.");
    }
}