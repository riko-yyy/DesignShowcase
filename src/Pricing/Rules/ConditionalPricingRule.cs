using DesignShowcase.Pricing;
using DesignShowcase.Pricing.Conditions;
using DesignShowcase.Results;

namespace DesignShowcase.Pricing.Rules;

/// <summary>
/// 条件(<see cref="IPricingCondition"/>)を満たした場合だけ、内部のルールを適用する。
/// 条件を満たさない場合は、価格をそのまま(調整なし)成功として返す。
///
/// 「ルール本体」と「適用条件」を合成で組み合わせる形にすることで、
/// 同じ条件を複数のルールに使い回したり、同じルールを別の条件と組み合わせたり
/// できるようにしている。
/// </summary>
public sealed class ConditionalPricingRule : IPricingRule
{
    private readonly IPricingCondition _condition;
    private readonly IPricingRule _rule;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="rule"></param>
    public ConditionalPricingRule(IPricingCondition condition, IPricingRule rule)
    {
        _condition = condition;
        _rule = rule;
    }

    /// <summary>
    /// 条件を満たした場合に対応する金額の調整を行い、満たさない場合は価格はそのまま調整なしとする
    /// </summary>
    /// <param name="price"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Result<Money> Apply(Money price, PricingContext context) =>
        _condition.IsSatisfiedBy(context)
            ? _rule.Apply(price, context)
            : Result<Money>.Success(price);
}
