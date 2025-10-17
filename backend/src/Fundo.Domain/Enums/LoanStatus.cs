using System.Runtime.Serialization;

namespace Fundo.Domain.Enums
{
    public enum LoanStatus
    {
        [EnumMember(Value = "Active")] Active,

        [EnumMember(Value = "Paid")] Paid
    }
}