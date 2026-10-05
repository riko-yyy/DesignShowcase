namespace DesignShowcase.BuildingBlocks;

/// <summary>
/// 値オブジェクトの基底クラス。
/// Entityとは対照的に、識別子を持たず「構成要素がすべて等しければ同一」という性質を表現する。
/// サブクラスはコンストラクタで不変条件(Invariant)を検証し、生成後は変更不可にすることを想定している。
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// 等価性判定・ハッシュ計算に使う構成要素を、宣言した順序で列挙する。
    /// サブクラスは自身のプロパティをそのまま返すだけでよく、
    /// Equals/GetHashCodeの実装をサブクラス側で個別に書かせない。
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// 値オブジェクトの等価性判定
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// 値オブジェクトの等価性判定
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as ValueObject);

    /// <summary>
    /// 値オブジェクトのハッシュコード
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // 構成要素の並び順に意味を持たせる(実装をシンプルに保つためのトレードオフ)。
        // 並び順を無視した等価性(集合的な比較)が必要な場合はサブクラス側でオーバーライドする想定。
        var hash = new HashCode();
        foreach (var component in GetEqualityComponents())
        {
            hash.Add(component);
        }
        return hash.ToHashCode();
    }

    /// <summary>
    /// 値オブジェクトの演算子による等価性判定
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    /// <summary>
    /// 値オブジェクトの演算子による不等価性判定
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}
