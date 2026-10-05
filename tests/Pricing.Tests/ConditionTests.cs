using DesignShowcase.Pricing;
using DesignShowcase.Pricing.Conditions;
using Xunit;

namespace DesignShowcase.Pricing.Tests;

public class ConditionTests
{
    [Fact]
    public void MinimumQuantityCondition_指定数量以上なら満たす()
    {
        var condition = new MinimumQuantityCondition(3);

        Assert.True(condition.IsSatisfiedBy(new PricingContext(quantity: 3, isMember: false)));
        Assert.True(condition.IsSatisfiedBy(new PricingContext(quantity: 5, isMember: false)));
    }

    [Fact]
    public void MinimumQuantityCondition_指定数量未満なら満たさない()
    {
        var condition = new MinimumQuantityCondition(3);

        Assert.False(condition.IsSatisfiedBy(new PricingContext(quantity: 2, isMember: false)));
    }

    [Fact]
    public void MemberOnlyCondition_会員なら満たす()
    {
        var condition = new MemberOnlyCondition();

        Assert.True(condition.IsSatisfiedBy(new PricingContext(quantity: 1, isMember: true)));
    }

    [Fact]
    public void MemberOnlyCondition_非会員なら満たさない()
    {
        var condition = new MemberOnlyCondition();

        Assert.False(condition.IsSatisfiedBy(new PricingContext(quantity: 1, isMember: false)));
    }
}
