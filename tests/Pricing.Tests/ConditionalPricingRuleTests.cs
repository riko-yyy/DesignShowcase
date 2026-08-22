using DesignShowcase.Pricing;
using DesignShowcase.Pricing.Conditions;
using DesignShowcase.Pricing.Rules;
using Xunit;

namespace DesignShowcase.Pricing.Tests;

public class ConditionalPricingRuleTests
{
    [Fact]
    public void 条件を満たせばルールが適用される()
    {
        var rule = new ConditionalPricingRule(
            new MemberOnlyCondition(),
            new PercentageDiscountRule(10));

        var result = rule.Apply(new Money(1000, "JPY"), new PricingContext(quantity: 1, isMember: true));

        Assert.True(result.IsSuccess);
        Assert.Equal(900, result.Value.Amount);
    }

    [Fact]
    public void 条件を満たさなければ価格はそのまま()
    {
        var rule = new ConditionalPricingRule(
            new MemberOnlyCondition(),
            new PercentageDiscountRule(10));

        var result = rule.Apply(new Money(1000, "JPY"), new PricingContext(quantity: 1, isMember: false));

        Assert.True(result.IsSuccess);
        Assert.Equal(1000, result.Value.Amount);
    }
}