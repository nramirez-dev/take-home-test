using Fundo.Domain.Enums;

namespace Fundo.Services.DTOs;

public class LoanDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public decimal CurrentBalance { get; set; }
    public string ApplicantName { get; set; }
    public LoanStatus Status { get; set; }
}