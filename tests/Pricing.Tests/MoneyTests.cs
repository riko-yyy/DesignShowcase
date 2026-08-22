using DesignShowcase.Pricing;
using Xunit;

namespace DesignShowcase.Pricing.Tests;

public class MoneyTests
{
    [Fact]
    public void AmountとCurrencyが同じなら等価()
    {
        var a = new Money(1000, "JPY");
        var b = new Money(1000, "JPY");

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Currencyが違えば非等価()
    {
        var a = new Money(1000, "JPY");
        var b = new Money(1000, "USD");

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void Amountが違えば非等価()
    {
        var a = new Money(1000, "JPY");
        var b = new Money(2000, "JPY");

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void nullとは非等価()
    {
        var money = new Money(1000, "JPY");

        Assert.False(money.Equals(null));
        Assert.False(money == null);
    }

    [Fact]
    public void 参照が同じなら等価()
    {
        var money = new Money(1000, "JPY");

        Assert.Equal(money, money);
    }

    [Fact]
    public void 負の金額は例外()
    {
        Assert.Throws<ArgumentException>(() => new Money(-1, "JPY"));
    }

    [Fact]
    public void Currencyが空文字は例外()
    {
        Assert.Throws<ArgumentException>(() => new Money(1000, ""));
    }

    [Fact]
    public void Currencyが空白のみでも例外()
    {
        Assert.Throws<ArgumentException>(() => new Money(1000, "   "));
    }

    [Fact]
    public void ゼロは許容される()
    {
        var money = new Money(0, "JPY");

        Assert.Equal(0, money.Amount);
    }

    [Fact]
    public void ToStringはAmountとCurrencyを含む()
    {
        var money = new Money(1000, "JPY");

        Assert.Equal("1000 JPY", money.ToString());
    }
}