using DesignShowcase.BuildingBlocks;

namespace DesignShowcase.Pricing;

/// <summary>
/// 料金調整ルールが判断材料として使う文脈情報。
/// 「何個買ったか」「会員かどうか」など、条件判定(IPricingCondition)や
/// ルール自体が参照する値をまとめて持たせている。
///
/// 新しい条件を追加したくなったらここにプロパティを増やす形になるため、
/// 意図的に必要最小限の項目だけに絞っている。
/// </summary>
public sealed class PricingContext : ValueObject
{
    /// <summary>
    /// 数量
    /// </summary>
    public int Quantity { get; }
    /// <summary>
    /// 会員かどうか
    /// </summary>
    public bool IsMember { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="isMember"></param>
    /// <exception cref="ArgumentException"></exception>
    public PricingContext(int quantity, bool isMember)
    {
        if (quantity < 0)
        {
            throw new ArgumentException("Quantityは0以上である必要があります。", nameof(quantity));
        }

        Quantity = quantity;
        IsMember = isMember;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Quantity;
        yield return IsMember;
    }
}
