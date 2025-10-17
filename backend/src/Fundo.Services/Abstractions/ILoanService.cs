using Fundo.Services.DTOs;

namespace Fundo.Services.Abstractions;

public interface ILoanService
{
    Task<LoanDto> CreateLoanAsync(CreateLoanDto dto);
    Task<LoanDto?> GetLoanByIdAsync(Guid id);
    Task<IEnumerable<LoanDto>> GetAllLoansAsync();
    Task<LoanDto?> ApplyPaymentAsync(Guid id, PaymentDto payment);
}