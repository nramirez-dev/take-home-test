using Fundo.Domain.Enums;
using Fundo.Domain.ValueObjects;

namespace Fundo.Domain.Entities;

public class Loan
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Money Amount { get; private set; }

    public Money CurrentBalance { get; private set; }

    public ApplicantName ApplicantName { get; private set; }

    public LoanStatus Status { get; private set; } = LoanStatus.Active;

    public Loan(Money amount, ApplicantName applicantName)
    {
        Amount = amount;
        CurrentBalance = amount;
        ApplicantName = applicantName;
        Status = LoanStatus.Active;
    }
     
    public void ApplyPayment(Money payment)
    {
        if (payment.Value <= 0 || payment > CurrentBalance)
            return;

        CurrentBalance -= payment;

        if (CurrentBalance.IsZero)
            Status = LoanStatus.Paid;
    }
}