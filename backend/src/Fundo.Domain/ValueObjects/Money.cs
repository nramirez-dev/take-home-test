namespace Fundo.Domain.ValueObjects;

public readonly struct Money
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString("C2");

    public bool IsGreaterThan(Money other) => Value > other.Value;
    public bool IsZero => Value == 0;

    public static Money operator -(Money a, Money b) => new(a.Value - b.Value);
    public static bool operator >(Money a, Money b) => a.Value > b.Value;
    public static bool operator <(Money a, Money b) => a.Value < b.Value;
}