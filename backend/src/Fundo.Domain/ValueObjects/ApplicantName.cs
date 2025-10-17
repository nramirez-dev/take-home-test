namespace Fundo.Domain.ValueObjects;

public class ApplicantName
{
    public string Value { get; }

    public ApplicantName(string value)
    {
        Value = value.Trim();
    }

    public override string ToString() => Value;
}