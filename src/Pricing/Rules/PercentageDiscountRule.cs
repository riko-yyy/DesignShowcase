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

    public PercentageDiscountRule(decimal percentageOff)
    {
        if (percentageOff is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(percentageOff), percentageOff, "割引率は0〜100の範囲で指定してください。");
        }

        _inner = new PercentageAdjustmentRule(-percentageOff);
    }

    public Result<Money> Apply(Money price, PricingContext context) =>
        _inner.Apply(price, context);
}