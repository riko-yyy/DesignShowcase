using DesignShowcase.Results;
using Xunit;

namespace DesignShowcase.Results.Tests;

public class ResultOfTTests
{
    private static readonly Error SampleError = new("Sample.Error", "サンプルエラー");

    [Fact]
    public void Successは値を保持する()
    {
        var result = Result<int>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void 失敗結果からValueを取得すると例外()
    {
        var result = Result<int>.Failure(SampleError);

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void Map_成功していれば値を変換する()
    {
        var result = Result<int>.Success(10).Map(x => x * 2);

        Assert.True(result.IsSuccess);
        Assert.Equal(20, result.Value);
    }

    [Fact]
    public void Map_失敗していれば変換関数は実行されずErrorを引き継ぐ()
    {
        var called = false;

        var result = Result<int>.Failure(SampleError).Map(x =>
        {
            called = true;
            return x * 2;
        });

        Assert.False(called);
        Assert.True(result.IsFailure);
        Assert.Equal(SampleError, result.Error);
    }

    [Fact]
    public void Bind_成功していれば次のResultにつながる()
    {
        var result = Result<int>.Success(10)
            .Bind(x => Result<string>.Success($"value:{x}"));

        Assert.True(result.IsSuccess);
        Assert.Equal("value:10", result.Value);
    }

    [Fact]
    public void Bind_失敗していれば次の処理は実行されずErrorを引き継ぐ()
    {
        var called = false;

        var result = Result<int>.Failure(SampleError)
            .Bind(x =>
            {
                called = true;
                return Result<string>.Success($"value:{x}");
            });

        Assert.False(called);
        Assert.True(result.IsFailure);
        Assert.Equal(SampleError, result.Error);
    }

    [Fact]
    public void Bind_途中で失敗すればそこで打ち切られる()
    {
        var secondCalled = false;

        var result = Result<int>.Success(10)
            .Bind(_ => Result<int>.Failure(SampleError))
            .Bind(x =>
            {
                secondCalled = true;
                return Result<int>.Success(x * 2);
            });

        Assert.False(secondCalled);
        Assert.True(result.IsFailure);
        Assert.Equal(SampleError, result.Error);
    }

    [Fact]
    public void 暗黙変換で値からResultを生成できる()
    {
        Result<int> result = 42;

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }
}
