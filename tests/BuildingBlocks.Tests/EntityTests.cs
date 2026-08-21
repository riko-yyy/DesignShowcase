using DesignShowcase.BuildingBlocks;
using Xunit;

namespace DesignShowcase.BuildingBlocks.Tests;

public class EntityTests
{
    // テスト用の最小限のエンティティ2種類。
    // 「型が違えば同じIdでも別物」という仕様を検証するために2種類用意している。
    private sealed class Customer : Entity<Guid>
    {
        public Customer(Guid id) : base(id) { }
    }

    private sealed class Order : Entity<Guid>
    {
        public Order(Guid id) : base(id) { }
    }

    // Guidのようなstructはnullを表現できないため、
    // null検証だけは参照型Id(string)のエンティティで別途確認する。
    private sealed class Ticket : Entity<string>
    {
        public Ticket(string id) : base(id) { }
    }

    [Fact]
    public void 同じ型で同じIdなら等価()
    {
        var id = Guid.NewGuid();
        var a = new Customer(id);
        var b = new Customer(id);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void 同じ型でも別Idなら非等価()
    {
        var a = new Customer(Guid.NewGuid());
        var b = new Customer(Guid.NewGuid());

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void 型が違えば同じIdでも非等価()
    {
        var id = Guid.NewGuid();
        var customer = new Customer(id);
        var order = new Order(id);

        // Entity<Guid>としては比較できてしまうが、意図的にfalseにしている点が設計上の要。
        Assert.False(customer.Equals(order));
    }

    [Fact]
    public void nullとは非等価()
    {
        var customer = new Customer(Guid.NewGuid());

        Assert.False(customer.Equals(null));
        Assert.False(customer == null);
    }

    [Fact]
    public void 同一参照なら等価()
    {
        var customer = new Customer(Guid.NewGuid());

        Assert.Equal(customer, customer);
    }

    [Fact]
    public void Idにnullを渡すと例外()
    {
        // Guidはstructなのでnullを表現できないため、参照型(string)Idで検証する。
        Assert.Throws<ArgumentNullException>(() => new Ticket(null!));
    }
}