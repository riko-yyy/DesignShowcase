using DesignShowcase.Pricing;
using DesignShowcase.Results;

namespace DesignShowcase.Pricing.Rules;

/// <summary>
/// 料金を調整するルールの共通インターフェース(Strategyパターン)。
///
/// 「調整した結果、金額がマイナスになる」というのは想定される業務上の失敗であり、
/// 例外ではなくResultで表現する。新しいルールを追加したい場合は、既存のルールや
/// CompositePricingRuleを一切変更せず、このインターフェースを実装するクラスを
/// 追加するだけでよい(Open-Closed原則)。
/// </summary>
public interface IPricingRule
{
    Result<Money> Apply(Money price, PricingContext context);
}