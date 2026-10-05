using DesignShowcase.Pricing;
using DesignShowcase.Pricing.Conditions;
using DesignShowcase.Pricing.Rules;
using DesignShowcase.Results;
using Xunit;

namespace DesignShowcase.Pricing.Tests;

public class CompositePricingRuleTests
{
    private static readonly PricingContext DefaultContext = new(quantity: 1, isMember: false);

    [Fact]
    public void 複数のルールが順番に適用される()
    {
        var rule = new CompositePricingRule(new IPricingRule[]
        {
            new PercentageDiscountRule(10),   // 1000 -> 900
            new FixedAmountAdjustmentRule(-100), // 900 -> 800
        });

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        Assert.Equal(800, result.Value.Amount);
    }

    [Fact]
    public void 途中のルールが失敗すればそこで打ち切られる()
    {
        var rule = new CompositePricingRule(new IPricingRule[]
        {
            new FixedAmountAdjustmentRule(-2000), // 1000 -> -1000 (失敗)
            new PercentageDiscountRule(50),        // ここには到達しない
        });

        var result = rule.Apply(new Money(1000, "JPY"), DefaultContext);

        Assert.True(result.IsFailure);
        Assert.Equal("Pricing.NegativeResult", result.Error.Code);
    }

    [Fact]
    public void 条件付きルールと組み合わせても動作する()
    {
        var rule = new CompositePricingRule(new IPricingRule[]
        {
            new ConditionalPricingRule(new MemberOnlyCondition(), new PercentageDiscountRule(10)),
            new ConditionalPricingRule(new MinimumQuantityCondition(5), new PercentageDiscountRule(5)),
        });

        var context = new PricingContext(quantity: 10, isMember: true);

        // 1000 -> (会員10%引き) 900 -> (5個以上5%引き) 855
        var result = rule.Apply(new Money(1000, "JPY"), context);

        Assert.True(result.IsSuccess);
        Assert.Equal(855, result.Value.Amount);
    }

    /// <summary>
    /// Open-Closed原則の実演。
    /// CompositePricingRuleや既存のルールクラスを一切変更せずに、
    /// このテストの中だけで定義した新しいルールを組み合わせられることを確認する。
    /// </summary>
    [Fact]
    public void 既存コードを変更せずに新しいルールを追加できる()
    {
        var rule = new CompositePricingRule(new IPricingRule[]
        {
            new PercentageDiscountRule(10),
            new RoundDownToNearestHundredRule(), // このテストで新しく定義したルール
        });

        var result = rule.Apply(new Money(1050, "JPY"), DefaultContext);

        Assert.True(result.IsSuccess);
        // 1050 -> (10%引き) 945 -> (100円単位で切り捨て) 900
        Assert.Equal(900, result.Value.Amount);
    }

    /// <summary>
    /// 「100円単位で切り捨てる」という、当初想定していなかった新しいルール。
    /// IPricingRuleを実装するだけで、既存のCompositePricingRuleや他のルールには
    /// 一切手を加えていない。
    /// </summary>
    private sealed class RoundDownToNearestHundredRule : IPricingRule
    {
        public Result<Money> Apply(Money price, PricingContext context)
        {
            var rounded = Math.Floor(price.Amount / 100) * 100;
            return Result<Money>.Success(new Money(rounded, price.Currency));
        }
    }
}
