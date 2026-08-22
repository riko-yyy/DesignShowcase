using DesignShowcase.Pricing;
using DesignShowcase.Results;

namespace DesignShowcase.Pricing.Rules;

/// <summary>
/// 複数の<see cref="IPricingRule"/>を順番に適用する(Compositeパターン)。
///
/// 各ルールの適用結果はResultsライブラリのBindでつないでおり、
/// 途中のルールが失敗(金額がマイナスになるなど)すれば、その時点で
/// 残りのルールは実行されず、失敗がそのまま呼び出し側に伝播する。
///
/// 新しい種類のルールを追加したい場合、このクラスは一切変更する必要がない
/// (IPricingRuleを実装した新しいクラスを作り、コンストラクタに渡すリストに
/// 追加するだけでよい)。
/// </summary>
public sealed class CompositePricingRule : IPricingRule
{
    private readonly IReadOnlyList<IPricingRule> _rules;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="rules"></param>
    public CompositePricingRule(IEnumerable<IPricingRule> rules)
    {
        _rules = rules.ToList();
    }

    /// <summary>
    /// ルールを順に適用し、金額を調整する
    /// </summary>
    /// <param name="price"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Result<Money> Apply(Money price, PricingContext context) =>
        _rules.Aggregate(
            Result<Money>.Success(price),
            (acc, rule) => acc.Bind(currentPrice => rule.Apply(currentPrice, context)));
}