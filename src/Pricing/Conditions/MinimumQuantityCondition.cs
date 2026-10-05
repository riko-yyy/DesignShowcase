using DesignShowcase.Pricing;

namespace DesignShowcase.Pricing.Conditions;

/// <summary>
/// 「指定した個数以上を購入している場合」という条件。
/// </summary>
public sealed class MinimumQuantityCondition : IPricingCondition
{
    /// <summary>
    /// 指定した個数
    /// </summary>
    private readonly int _minimumQuantity;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="minimumQuantity"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public MinimumQuantityCondition(int minimumQuantity)
    {
        if (minimumQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(minimumQuantity), minimumQuantity, "0以上で指定してください。");
        }

        _minimumQuantity = minimumQuantity;
    }

    /// <summary>
    /// 指定した個数以上を購入している場合は条件を満たす
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public bool IsSatisfiedBy(PricingContext context) => context.Quantity >= _minimumQuantity;
}
