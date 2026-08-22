using DesignShowcase.Pricing;
using DesignShowcase.Results;

namespace DesignShowcase.Pricing.Rules;

/// <summary>
/// 価格に対して固定額を加減するルール。
/// <paramref name="amount"/>に負の値を渡せば固定額の値引き、正の値を渡せば
/// 固定額の加算になる。PercentageAdjustmentRuleと同様、符号だけで
/// 割引/増額の両方を表現できる設計にしている。
/// </summary>
public sealed class FixedAmountAdjustmentRule : IPricingRule
{
    private readonly decimal _amount;

    public FixedAmountAdjustmentRule(decimal amount)
    {
        _amount = amount;
    }

    public Result<Money> Apply(Money price, PricingContext context)
    {
        var newAmount = price.Amount + _amount;

        if (newAmount < 0)
        {
            return Result<Money>.Failure(new Error(
                "Pricing.NegativeResult",
                $"調整後の金額がマイナスになりました(調整前: {price.Amount}, 調整額: {_amount})。"));
        }

        return Result<Money>.Success(new Money(newAmount, price.Currency));
    }
}