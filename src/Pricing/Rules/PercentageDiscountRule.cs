using DesignShowcase.Pricing;
using DesignShowcase.Results;

namespace DesignShowcase.Pricing.Rules;

/// <summary>
/// 割合(%)で価格を割り引くルール。
///
/// 実体は<see cref="PercentageAdjustmentRule"/>に負の値を渡しているだけであり、
/// 計算ロジックそのものはここには存在しない。存在理由は「割引である」という意図を
/// クラス名で明確に伝えること。
/// </summary>
public sealed class PercentageDiscountRule : IPricingRule
{
    private readonly PercentageAdjustmentRule _inner;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="percentageOff"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public PercentageDiscountRule(decimal percentageOff)
    {
        if (percentageOff is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(percentageOff), percentageOff, "割引率は0〜100の範囲で指定してください。");
        }

        _inner = new PercentageAdjustmentRule(-percentageOff);
    }

    /// <summary>
    /// 割合に応じて割引し、料金を調整する
    /// </summary>
    /// <param name="price"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Result<Money> Apply(Money price, PricingContext context) =>
        _inner.Apply(price, context);
}