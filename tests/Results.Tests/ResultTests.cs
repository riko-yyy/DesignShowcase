using DesignShowcase.Results;
using Xunit;

namespace DesignShowcase.Results.Tests;

public class ResultTests
{
    private static readonly Error SampleError = new("Sample.Error", "サンプルエラー");

    [Fact]
    public void Successは成功状態でErrorを持たない()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failureは失敗状態で指定したErrorを持つ()
    {
        var result = Result.Failure(SampleError);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(SampleError, result.Error);
    }

    [Fact]
    public void Bind_成功していれば次の処理が実行される()
    {
        var called = false;

        var result = Result.Success().Bind(() =>
        {
            called = true;
            return Result.Success();
        });

        Assert.True(called);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Bind_失敗していれば次の処理は実行されず失敗を維持する()
    {
        var called = false;

        var result = Result.Failure(SampleError).Bind(() =>
        {
            called = true;
            return Result.Success();
        });

        Assert.False(called);
        Assert.True(result.IsFailure);
        Assert.Equal(SampleError, result.Error);
    }

    [Fact]
    public void ResultへのBindで失敗時はErrorを引き継ぐ()
    {
        var result = Result.Failure(SampleError).Bind(() => Result.Success(42));

        Assert.True(result.IsFailure);
        Assert.Equal(SampleError, result.Error);
    }
}