using DesignShowcase.Pricing;
using DesignShowcase.Results;

namespace DesignShowcase.Pricing.Rules;

/// <summary>
/// 割合(%)で価格を増額(エンハンス)するルール。
///
/// <see cref="PercentageDiscountRule"/>と全く同じ<see cref="PercentageAdjustmentRule"/>を
/// 使っており、符号が逆(正の値)であること以外の違いはない。
/// 「割引の逆」を新しい概念として一から実装するのではなく、
/// 同じ計算エンジンをクラス名だけ変えて再利用できることを示す例。
/// </summary>
public sealed class PercentageEnhancementRule : IPricingRule
{
    private readonly PercentageAdjustmentRule _inner;

    public PercentageEnhancementRule(decimal percentageBoost)
    {
        if (percentageBoost < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(percentageBoost), percentageBoost, "増額率は0以上で指定してください。");
        }

        _inner = new PercentageAdjustmentRule(percentageBoost);
    }

    public Result<Money> Apply(Money price, PricingContext context) =>
        _inner.Apply(price, context);
}