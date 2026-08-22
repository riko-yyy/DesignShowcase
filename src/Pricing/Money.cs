using DesignShowcase.BuildingBlocks;

namespace DesignShowcase.Pricing;

/// <summary>
/// 金額を表す値オブジェクト。
/// BuildingBlocksのValueObjectを継承し、AmountとCurrencyの組で等価性が決まる。
///
/// Amountが負の値になることは「そもそも成立しないデータ」なのでコンストラクタで
/// 例外にしている。一方、料金調整の結果として金額がマイナスになるかどうかは
/// 「起こりうる業務上の失敗」なので、各PricingRuleの側でResultとして扱う
/// (このMoneyコンストラクタを負の値で呼ばないように、呼ぶ前に検証する)。
/// </summary>
public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currencyは必須です。", nameof(currency));
        }

        if (amount < 0)
        {
            throw new ArgumentException("Amountは0以上である必要があります。", nameof(amount));
        }

        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount} {Currency}";
}