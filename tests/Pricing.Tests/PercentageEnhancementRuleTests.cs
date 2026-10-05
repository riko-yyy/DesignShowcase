using DesignShowcase.Pricing;
using DesignShowcase.Pricing.Rules;
using Xunit;

namespace DesignShowcase.Pricing.Tests;

public class PercentageEnhancementRuleTests
{
    private static readonly PricingContext DefaultContext = new(quantity: 1, isMember: false);

    [Fact]
    public void 価格を割合分増やす()
    {
        var rule = new PercentageEnhancementRule(percentageBoost: 20);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal(1200, result.Value.Amount);
    }

    [Fact]
    public void 増額率が負だと生成時に例外()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PercentageEnhancementRule(-1));
    }

    [Fact]
    public void 増額率がゼロなら価格は変わらない()
    {
        var rule = new PercentageEnhancementRule(0);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal(1000, result.Value.Amount);
    }

    [Fact]
    public void 通貨は調整後も維持される()
    {
        var rule = new PercentageEnhancementRule(10);

        var result = rule.Apply(new Money(1000, "USD"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal("USD", result.Value.Currency);
    }
}
