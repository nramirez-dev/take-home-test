using System.Text.Json.Serialization;
using Fundo.Domain.Enums;
using Fundo.Shared.Helper;

namespace Fundo.Services.DTOs;

public class LoanDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public decimal CurrentBalance { get; set; }
    public string ApplicantName { get; set; }

    [JsonConverter(typeof(EnumMemberJsonConverter<LoanStatus>))]
    public LoanStatus Status { get; set; }
}