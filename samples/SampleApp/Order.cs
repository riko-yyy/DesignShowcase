using DesignShowcase.BuildingBlocks;
using DesignShowcase.Pricing;
using DesignShowcase.Results;

namespace DesignShowcase.SampleApp;

/// <summary>
/// 3つのライブラリを組み合わせるための最小限の注文エンティティ。
/// BuildingBlocksの<see cref="Entity{TId}"/>を継承し、単価は
/// Pricingが提供する<see cref="Money"/>として保持する。
/// </summary>
public sealed class Order : Entity<Guid>
{
    public Money UnitPrice { get; }
    public int Quantity { get; }
    public bool IsMember { get; }

    private Order(Guid id, Money unitPrice, int quantity, bool isMember) : base(id)
    {
        UnitPrice = unitPrice;
        Quantity = quantity;
        IsMember = isMember;
    }

    /// <summary>
    /// 数量が1以上であることを検証してから生成する。
    /// 「数量が0以下」は起こりうる入力ミスなので、例外ではなくResultで表現する。
    /// </summary>
    public static Result<Order> Create(Money unitPrice, int quantity, bool isMember)
    {
        if (quantity <= 0)
        {
            return Result<Order>.Failure(new Error(
                "Order.InvalidQuantity", "数量は1以上である必要があります。"));
        }

        return Result<Order>.Success(new Order(Guid.NewGuid(), unitPrice, quantity, isMember));
    }

    /// <summary>単価 × 数量の小計。</summary>
    public Money CalculateSubtotal() => new(UnitPrice.Amount * Quantity, UnitPrice.Currency);
}