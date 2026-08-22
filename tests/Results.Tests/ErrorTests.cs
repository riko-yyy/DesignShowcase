using DesignShowcase.Results;
using Xunit;

namespace DesignShowcase.Results.Tests;

public class ErrorTests
{
    [Fact]
    public void CodeとMessageが同じなら等価()
    {
        var a = new Error("Order.NotFound", "注文が見つかりません");
        var b = new Error("Order.NotFound", "注文が見つかりません");

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Codeが違えば非等価()
    {
        var a = new Error("Order.NotFound", "同じメッセージ");
        var b = new Error("Order.AlreadyShipped", "同じメッセージ");

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void Messageが違えば非等価()
    {
        var a = new Error("Order.NotFound", "メッセージA");
        var b = new Error("Order.NotFound", "メッセージB");

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void NoneはCodeとMessageが空文字()
    {
        Assert.Equal(string.Empty, Error.None.Code);
        Assert.Equal(string.Empty, Error.None.Message);
    }

    [Fact]
    public void 同じ内容ならNoneと等価になる()
    {
        var sameAsNone = new Error(string.Empty, string.Empty);

        Assert.Equal(Error.None, sameAsNone);
    }

    [Fact]
    public void objectとしての比較でも同様に等価判定できる()
    {
        var a = new Error("Order.NotFound", "注文が見つかりません");
        object b = new Error("Order.NotFound", "注文が見つかりません");

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Errorではないobjectとは非等価()
    {
        var a = new Error("Order.NotFound", "注文が見つかりません");

        Assert.False(a.Equals("Order.NotFound"));
    }

    [Fact]
    public void Codeにnullを渡すと例外()
    {
        Assert.Throws<ArgumentNullException>(() => new Error(null!, "メッセージ"));
    }

    [Fact]
    public void Messageにnullを渡すと例外()
    {
        Assert.Throws<ArgumentNullException>(() => new Error("Order.NotFound", null!));
    }

    [Fact]
    public void ToStringはNoneの場合noneを返す()
    {
        Assert.Equal("(none)", Error.None.ToString());
    }

    [Fact]
    public void ToStringはCodeとMessageを含む()
    {
        var error = new Error("Order.NotFound", "注文が見つかりません");

        Assert.Equal("Order.NotFound: 注文が見つかりません", error.ToString());
    }
}