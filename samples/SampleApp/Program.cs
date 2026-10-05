using DesignShowcase.Pricing;
using DesignShowcase.Pricing.Conditions;
using DesignShowcase.Pricing.Rules;
using DesignShowcase.Results;
using DesignShowcase.SampleApp;

// ==========================================================================
// 「入力検証(Results) → ドメインオブジェクト生成(BuildingBlocks)
//  → 料金計算(Pricing)」という一連の業務フローを、3つのライブラリを
// 組み合わせて表現するデモ。
//
// 本来はユーザー入力やAPIリクエストとして受け取る値を、あえて文字列のまま
// 用意している(バリデーションが本当に必要な場面を再現するため)。
// ==========================================================================

var rawUnitPrice = "1000";
var rawQuantity = "6";
const bool isMember = true;
const string currency = "JPY";

var result = OrderInputParser.ParseUnitPrice(rawUnitPrice)
    .Bind(unitPriceAmount => OrderInputParser.ParseQuantity(rawQuantity)
        .Bind(quantity => Order.Create(new Money(unitPriceAmount, currency), quantity, isMember)))
    .Bind(ApplyPricingRules);

if (result.IsSuccess)
{
    Console.WriteLine($"合計金額: {result.Value}");
}
else
{
    Console.WriteLine($"エラー[{result.Error.Code}]: {result.Error.Message}");
}

/// <summary>
/// 「会員なら10%引き」「5個以上なら追加で5%引き」という2つのルールを
/// CompositePricingRuleで組み合わせて注文の小計に適用する。
/// </summary>
static Result<Money> ApplyPricingRules(Order order)
{
    var pricingRule = new CompositePricingRule(new IPricingRule[]
    {
        new ConditionalPricingRule(new MemberOnlyCondition(), new PercentageDiscountRule(10)),
        new ConditionalPricingRule(new MinimumQuantityCondition(5), new PercentageDiscountRule(5)),
    });

    var context = new PricingContext(order.Quantity, order.IsMember);

    return pricingRule.Apply(order.CalculateSubtotal(), context);
}
