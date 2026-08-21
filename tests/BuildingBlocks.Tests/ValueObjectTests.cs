using DesignShowcase.BuildingBlocks;
using Xunit;

namespace DesignShowcase.BuildingBlocks.Tests;

public class ValueObjectTests
{
    // 「住所」のような、複数のプリミティブから構成される典型的な値オブジェクトを想定。
    private sealed class Address : ValueObject
    {
        public string PostalCode { get; }
        public string City { get; }

        public Address(string postalCode, string city)
        {
            PostalCode = postalCode;
            City = city;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return PostalCode;
            yield return City;
        }
    }

    // 構成要素の型・個数が異なる別の値オブジェクト(型を跨いだ等価性がないことの確認用)。
    private sealed class Money : ValueObject
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }

    [Fact]
    public void 構成要素がすべて同じなら等価()
    {
        var a = new Address("100-0001", "Chiyoda");
        var b = new Address("100-0001", "Chiyoda");

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void 構成要素が一つでも違えば非等価()
    {
        var a = new Address("100-0001", "Chiyoda");
        var b = new Address("100-0001", "Minato");

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void 型が違えば構成要素の型がたまたま同じでも非等価()
    {
        var address = new Address("100-0001", "Chiyoda");
        var money = new Money(1000, "JPY");

        // どちらも (string-like, string) の2要素だが、型が違うので比較不可として扱う。
        Assert.False(address.Equals(money));
    }

    [Fact]
    public void nullとは非等価()
    {
        var address = new Address("100-0001", "Chiyoda");

        Assert.False(address.Equals(null));
        Assert.False(address == null);
    }

    [Fact]
    public void 参照が同じなら等価()
    {
        var address = new Address("100-0001", "Chiyoda");

        Assert.Equal(address, address);
    }
}