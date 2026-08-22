namespace DesignShowcase.Results;

/// <summary>
/// 「処理が成功したか失敗したか」を表現する型。
///
/// 例外は「呼び出し側が回復できない・想定していない異常」のために温存し、
/// バリデーション違反やビジネスルール違反のような「想定内の業務的な失敗」は
/// この型で表現する、という役割分担を前提にしている。
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        // 「成功なのにErrorがある」「失敗なのにErrorがない」という
        // 矛盾した状態を作れないようにコンストラクタで強制する。
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("成功結果はErrorを持てません。");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("失敗結果はErrorを持つ必要があります。");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Success<T>(T value) => Result<T>.Success(value);

    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);

    /// <summary>
    /// 成功していれば次の処理を実行し、失敗していればその失敗をそのまま伝播する。
    /// 「途中で失敗したら以降の処理をスキップする」という制御フローを、
    /// if文の連鎖ではなくメソッドチェーンで表現するために用意している。
    /// </summary>
    public Result Bind(Func<Result> next) => IsSuccess ? next() : this;

    /// <summary>
    /// 成功していれば値を伴う処理につなげる。失敗していれば同じErrorを引き継いで
    /// Result&lt;T&gt;の失敗として伝播する。
    /// </summary>
    public Result<T> Bind<T>(Func<Result<T>> next) => IsSuccess ? next() : Result<T>.Failure(Error);
}