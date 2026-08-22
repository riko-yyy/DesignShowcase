using DesignShowcase.Results;
using Xunit;

namespace DesignShowcase.Results.Tests;

public class ResultExtensionsTests
{
    private static readonly Error SampleError = new("Sample.Error", "サンプルエラー");

    private static Task<Result<int>> SuccessAsync(int value) => Task.FromResult(Result<int>.Success(value));

    private static Task<Result<int>> FailureAsync() => Task.FromResult(Result<int>.Failure(SampleError));

    [Fact]
    public async Task MapAsync_成功していれば値を変換する()
    {
        var result = await SuccessAsync(10).MapAsync(x => x * 2);

        Assert.True(result.IsSuccess);
        Assert.Equal(20, result.Value);
    }

    [Fact]
    public async Task MapAsync_失敗していればErrorを引き継ぐ()
    {
        var result = await FailureAsync().MapAsync(x => x * 2);

        Assert.True(result.IsFailure);
        Assert.Equal(SampleError, result.Error);
    }

    [Fact]
    public async Task BindAsync_同期版のbinderにもつなげられる()
    {
        var result = await SuccessAsync(10)
            .BindAsync(x => Result<string>.Success($"value:{x}"));

        Assert.True(result.IsSuccess);
        Assert.Equal("value:10", result.Value);
    }

    [Fact]
    public async Task BindAsync_非同期のbinderにもつなげられる()
    {
        var result = await SuccessAsync(10)
            .BindAsync(x => SuccessAsync(x * 2).MapAsync(y => $"value:{y}"));

        Assert.True(result.IsSuccess);
        Assert.Equal("value:20", result.Value);
    }

    [Fact]
    public async Task BindAsync_途中で失敗すればそこで打ち切られる()
    {
        var called = false;

        var result = await FailureAsync()
            .BindAsync(x =>
            {
                called = true;
                return SuccessAsync(x * 2);
            });

        Assert.False(called);
        Assert.True(result.IsFailure);
        Assert.Equal(SampleError, result.Error);
    }
}