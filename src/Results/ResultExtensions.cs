namespace DesignShowcase.Results;

/// <summary>
/// Task&lt;Result&lt;T&gt;&gt;を扱う際に、毎回awaitしてからMap/Bindを呼ぶのではなく、
/// メソッドチェーンをそのまま続けられるようにするための拡張メソッド。
///
/// 例:
/// <code>
/// var result = await FetchOrderAsync(id)
///     .MapAsync(order => order.Total)
///     .BindAsync(total => ApplyDiscountAsync(total));
/// </code>
/// </summary>
public static class ResultExtensions
{
    public static async Task<Result<TNew>> MapAsync<T, TNew>(
        this Task<Result<T>> resultTask,
        Func<T, TNew> mapper)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Map(mapper);
    }

    public static async Task<Result<TNew>> BindAsync<T, TNew>(
        this Task<Result<T>> resultTask,
        Func<T, Result<TNew>> binder)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Bind(binder);
    }

    /// <summary>
    /// 次につなげる処理自体が非同期(Task&lt;Result&lt;TNew&gt;&gt;を返す)場合のオーバーロード。
    /// </summary>
    public static async Task<Result<TNew>> BindAsync<T, TNew>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TNew>>> binder)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.IsSuccess
            ? await binder(result.Value).ConfigureAwait(false)
            : Result<TNew>.Failure(result.Error);
    }
}