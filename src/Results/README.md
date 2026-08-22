# Results

例外に頼らず「処理が成功したか失敗したか」を型として表現するための軽量ライブラリです。

## 提供するもの

| 型 | 役割 |
|---|---|
| `Result` | 値を伴わない処理結果(成功/失敗のみ) |
| `Result<T>` | 値を伴う処理結果 |
| `Error` | 失敗の理由(Code + Message) |
| `ResultExtensions` | `Task<Result<T>>`をチェーンさせるための非同期拡張 |

## 例外とResultの役割分担

- **例外**: 呼び出し側が回復できない・想定していない異常(バグ、インフラ障害など)
- **Result**: 業務上想定される失敗(バリデーション違反、ビジネスルール違反など)

「本当に起きてはいけないこと」と「起きうることの一つ」を型で区別することで、
呼び出し側は`try-catch`ではなく`if (result.IsFailure)`で正常系のフローとして
失敗を扱えるようになります。

## Errorはなぜrecordを使わず自前実装なのか

`BuildingBlocks`の`ValueObject`と同じ「構造的等価性を持つ値」なので、
一貫性を優先し`Error`も自前実装(`IEquatable<Error>`を手動実装)にしています。

`record`を使えばコード量は大きく減りますが、C#固有の言語機能に頼った実装になるため、
「言語を問わず通用する設計思考を見せる」という本リポジトリの目的には
自前実装の方が合っていると判断しました。

## Bind / Map の使い分け

- `Map`: 成功時の値を**別の値に変換するだけ**(変換自体は失敗しない)
- `Bind`: 成功時の値を使って**別のResultを返す処理につなげる**(その処理自体が失敗しうる)

```csharp
Result<Order> order = GetOrder(id);

Result<decimal> total = order
    .Bind(o => ValidateOrder(o))       // 検証自体が失敗しうるのでBind
    .Map(o => o.CalculateTotal());     // 計算は失敗しないのでMap
```