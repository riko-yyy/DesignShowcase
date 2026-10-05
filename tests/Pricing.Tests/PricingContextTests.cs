using DesignShowcase.Pricing;
using Xunit;

namespace DesignShowcase.Pricing.Tests;

public class PricingContextTests
{
    [Fact]
    public void QuantityとIsMemberが同じなら等価()
    {
        var a = new PricingContext(quantity: 3, isMember: true);
        var b = new PricingContext(quantity: 3, isMember: true);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Quantityが違えば非等価()
    {
        var a = new PricingContext(quantity: 3, isMember: true);
        var b = new PricingContext(quantity: 5, isMember: true);

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void IsMemberが違えば非等価()
    {
        var a = new PricingContext(quantity: 3, isMember: true);
        var b = new PricingContext(quantity: 3, isMember: false);

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void nullとは非等価()
    {
        var context = new PricingContext(quantity: 1, isMember: false);

        Assert.False(context.Equals(null));
        Assert.False(context == null);
    }

    [Fact]
    public void 参照が同じなら等価()
    {
        var context = new PricingContext(quantity: 1, isMember: false);

        Assert.Equal(context, context);
    }

    [Fact]
    public void 負のQuantityは例外()
    {
        Assert.Throws<ArgumentException>(() => new PricingContext(quantity: -1, isMember: false));
    }

    [Fact]
    public void Quantityがゼロは許容される()
    {
        var context = new PricingContext(quantity: 0, isMember: false);

        Assert.Equal(0, context.Quantity);
    }
}
