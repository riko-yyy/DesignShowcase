using DesignShowcase.Pricing;
using DesignShowcase.Pricing.Rules;
using Xunit;

namespace DesignShowcase.Pricing.Tests;

public class PercentageAdjustmentRuleTests
{
    private static readonly PricingContext DefaultContext = new(quantity: 1, isMember: false);

    [Fact]
    public void 割引は価格を割合分減らす()
    {
        var rule = new PercentageDiscountRule(percentageOff: 20);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal(800, result.Value.Amount);
    }

    [Fact]
    public void 増額は価格を割合分増やす()
    {
        var rule = new PercentageEnhancementRule(percentageBoost: 20);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal(1200, result.Value.Amount);
    }

    [Fact]
    public void 割引率が100を超えると生成時に例外()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PercentageDiscountRule(101));
    }

    [Fact]
    public void 割引率が負だと生成時に例外()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PercentageDiscountRule(-1));
    }

    [Fact]
    public void 増額率が負だと生成時に例外()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PercentageEnhancementRule(-1));
    }

    [Fact]
    public void 割引100パーセントで0円になるのは成功扱い()
    {
        var rule = new PercentageDiscountRule(100);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.Amount);
    }

    [Fact]
    public void 通貨は調整後も維持される()
    {
        var rule = new PercentageDiscountRule(10);

        var result = rule.Apply(new Money(1000, "USD"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal("USD", result.Value.Currency);
    }
}