using DesignShowcase.Pricing;

namespace DesignShowcase.Pricing.Conditions;

/// <summary>
/// 「指定した個数以上を購入している場合」という条件。
/// </summary>
public sealed class MinimumQuantityCondition : IPricingCondition
{
    private readonly int _minimumQuantity;

    public MinimumQuantityCondition(int minimumQuantity)
    {
        if (minimumQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(minimumQuantity), minimumQuantity, "0以上で指定してください。");
        }

        _minimumQuantity = minimumQuantity;
    }

    public bool IsSatisfiedBy(PricingContext context) => context.Quantity >= _minimumQuantity;
}