using DesignShowcase.Pricing;

namespace DesignShowcase.Pricing.Conditions;

/// <summary>
/// 「会員である場合」という条件。
/// </summary>
public sealed class MemberOnlyCondition : IPricingCondition
{
    public bool IsSatisfiedBy(PricingContext context) => context.IsMember;
}