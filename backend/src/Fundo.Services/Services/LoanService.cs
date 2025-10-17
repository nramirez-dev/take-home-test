using Fundo.Domain.Entities;
using Fundo.Domain.ValueObjects;
using Fundo.Services.Abstractions;
using Fundo.Services.DTOs;

namespace Fundo.Services.Services;

public class LoanService : ILoanService
{
    private readonly IRepository<Loan> _loanRepository;

    public LoanService(IRepository<Loan> loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<LoanDto> CreateLoanAsync(CreateLoanDto dto)
    {
        var amount = new Money(dto.Amount);
        var applicant = new ApplicantName(dto.ApplicantName);
        var loan = new Loan(amount, applicant);

        await _loanRepository.AddAsync(loan);

        return MapToDto(loan);
    }

    public async Task<LoanDto?> GetLoanByIdAsync(Guid id)
    {
        var loan = await _loanRepository.GetByIdAsync(id);
        return loan == null ? null : MapToDto(loan);
    }

    public async Task<IEnumerable<LoanDto>> GetAllLoansAsync()
    {
        var loans = await _loanRepository.GetAllAsync();
        return loans.Select(MapToDto);
    }

    public async Task<LoanDto?> ApplyPaymentAsync(Guid id, PaymentDto payment)
    {
        var loan = await _loanRepository.GetByIdAsync(id);
        if (loan == null)
            return null;

        var paymentAmount = new Money(payment.PaymentAmount);
        loan.ApplyPayment(paymentAmount);

        await _loanRepository.UpdateAsync(loan);

        return MapToDto(loan);
    }

    private static LoanDto MapToDto(Loan loan)
    {
        return new LoanDto
        {
            Id = loan.Id,
            Amount = loan.Amount.Value,
            CurrentBalance = loan.CurrentBalance.Value,
            ApplicantName = loan.ApplicantName.Value,
            Status = loan.Status
        };
    }
}