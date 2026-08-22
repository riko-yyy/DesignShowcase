using DesignShowcase.Pricing;

namespace DesignShowcase.Pricing.Conditions;

/// <summary>
/// 「会員である場合」という条件。
/// </summary>
public sealed class MemberOnlyCondition : IPricingCondition
{
    /// <summary>
    /// 会員である場合は条件を満たす
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public bool IsSatisfiedBy(PricingContext context) => context.IsMember;
}