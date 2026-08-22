using DesignShowcase.Pricing;

namespace DesignShowcase.Pricing.Conditions;

/// <summary>
/// 料金調整ルールを適用してよいかどうかの条件(Specificationパターン)。
///
/// 「ルールそのもの(何をするか)」と「適用条件(いつするか)」を別の型に分けることで、
/// 同じ条件を複数のルールに使い回したり、条件だけを差し替えたりできるようにしている。
/// </summary>
public interface IPricingCondition
{
    /// <summary>
    /// 条件を満たしているかどうか
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    bool IsSatisfiedBy(PricingContext context);
}