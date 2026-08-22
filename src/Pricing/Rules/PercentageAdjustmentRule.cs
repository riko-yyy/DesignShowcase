using DesignShowcase.Pricing;
using DesignShowcase.Results;

namespace DesignShowcase.Pricing.Rules;

/// <summary>
/// 価格に対して割合(%)で調整を加えるルールの本体。
///
/// <paramref name="percentage"/>に負の値を渡せば割引、正の値を渡せば増額(エンハンス)
/// になる。「割引」と「増額」は符号が違うだけで計算ロジックは全く同じであるため、
/// このクラス自体を利用者に直接使わせるのではなく、
/// <see cref="PercentageDiscountRule"/>と<see cref="PercentageEnhancementRule"/>という
/// 意図の伝わる名前のクラスでラップして公開している。
///
/// 「直接使わせない」という意図をアクセス修飾子でも表明するため、internalにしている。
/// </summary>
internal sealed class PercentageAdjustmentRule : IPricingRule
{
    /// <summary>
    /// 調整する割合値（％）
    /// </summary>
    private readonly decimal _percentage;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="percentage"></param>
    public PercentageAdjustmentRule(decimal percentage)
    {
        _percentage = percentage;
    }

    /// <summary>
    /// 割合値に応じて料金を調整する
    /// </summary>
    /// <param name="price"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Result<Money> Apply(Money price, PricingContext context)
    {
        var adjustment = price.Amount * (_percentage / 100m);
        var newAmount = price.Amount + adjustment;

        if (newAmount < 0)
        {
            return Result<Money>.Failure(new Error(
                "Pricing.NegativeResult",
                $"調整後の金額がマイナスになりました(調整前: {price.Amount}, 割合: {_percentage}%)。"));
        }

        return Result<Money>.Success(new Money(newAmount, price.Currency));
    }
}