using DesignShowcase.BuildingBlocks;

namespace DesignShowcase.Pricing;

/// <summary>
/// 金額を表す値オブジェクト。
/// BuildingBlocksのValueObjectを継承し、AmountとCurrencyの組で等価性が決まる。
/// </summary>
public sealed class Money : ValueObject
{
    /// <summary>
    /// 金額
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// 通貨
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="currency"></param>
    /// <exception cref="ArgumentException"></exception>
    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currencyは必須です。", nameof(currency));
        }

        // Amountが負の値になることは「そもそも成立しないデータ」なのでコンストラクタで
        // 例外にしている。一方、料金調整の結果として金額がマイナスになるかどうかは
        // 「起こりうる業務上の失敗」なので、各PricingRuleの側でResultとして扱う
        // (このMoneyコンストラクタを負の値で呼ばないように、呼ぶ前に検証する)。
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
