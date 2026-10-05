using DesignShowcase.Pricing;
using DesignShowcase.Pricing.Rules;
using Xunit;

namespace DesignShowcase.Pricing.Tests;

public class FixedAmountAdjustmentRuleTests
{
    private static readonly PricingContext DefaultContext = new(quantity: 1, isMember: false);

    [Fact]
    public void 正の値なら固定額を加算する()
    {
        var rule = new FixedAmountAdjustmentRule(500);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal(1500, result.Value.Amount);
    }

    [Fact]
    public void 負の値なら固定額を減算する()
    {
        var rule = new FixedAmountAdjustmentRule(-300);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal(700, result.Value.Amount);
    }

    [Fact]
    public void 価格を超える値引きは失敗として扱われる()
    {
        var rule = new FixedAmountAdjustmentRule(-1500);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsFailure);
        Assert.Equal("Pricing.NegativeResult", result.Error.Code);
    }

    [Fact]
    public void 失敗時にValueへアクセスすると例外()
    {
        var rule = new FixedAmountAdjustmentRule(-1500);

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
