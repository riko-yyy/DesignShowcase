using DesignShowcase.Results;

namespace DesignShowcase.SampleApp;

/// <summary>
/// ユーザー入力(文字列)を検証しながら値に変換する。
/// 「入力ミス」は例外ではなくResultで表現し、後続の処理とBindでつなげる。
/// </summary>
public static class OrderInputParser
{
    /// <summary>
    /// 文字列の単価をパースしてその結果を返す
    /// </summary>
    /// <param name="raw"></param>
    /// <returns></returns>
    public static Result<decimal> ParseUnitPrice(string raw)
    {
        if (!decimal.TryParse(raw, out var value))
        {
            return Result<decimal>.Failure(new Error(
                "Input.InvalidUnitPrice", $"単価として解釈できません: '{raw}'"));
        }

        if (value < 0)
        {
            return Result<decimal>.Failure(new Error(
                "Input.NegativeUnitPrice", "単価は0以上である必要があります。"));
        }

        return Result<decimal>.Success(value);
    }

    /// <summary>
    /// 文字列の数量をパースしてその結果を返す
    /// </summary>
    /// <param name="raw"></param>
    /// <returns></returns>
    public static Result<int> ParseQuantity(string raw)
    {
        if (!int.TryParse(raw, out var value))
        {
            return Result<int>.Failure(new Error(
                "Input.InvalidQuantity", $"数量として解釈できません: '{raw}'"));
        }

        return Result<int>.Success(value);
    }
}