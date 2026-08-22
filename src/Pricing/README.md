# Pricing

ルールを合成して価格を算出するエンジンです。割引・割増(エンハンス)のどちらも、
同じ仕組みの上で表現できるように設計しています。

`BuildingBlocks`(値オブジェクトの基盤)と`Results`(失敗の表現)の両方を
実際に組み合わせて使っている、3ライブラリの中で唯一「他の2つに依存する」ライブラリです。

## 提供するもの

| 型 | 役割 |
|---|---|
| `Money` | 金額を表す値オブジェクト(`BuildingBlocks.ValueObject`を継承) |
| `PricingContext` | ルール適用の判断材料(数量・会員かどうかなど) |
| `IPricingRule` | 価格調整ルールの共通インターフェース(Strategy) |
| `IPricingCondition` | ルールを適用してよいかの条件(Specification) |
| `ConditionalPricingRule` | 条件を満たした時だけ内部のルールを適用する |
| `CompositePricingRule` | 複数のルールを順に適用する(Composite) |
| `PercentageAdjustmentRule` | 割合(%)で価格を調整する計算の本体 |
| `PercentageDiscountRule` / `PercentageEnhancementRule` | 上記を割引・増額の名前でラップしたもの |
| `FixedAmountAdjustmentRule` | 固定額で価格を調整する |

## 割引とエンハンスは同じ計算エンジンを共有している

`PercentageAdjustmentRule`は「割合で価格を増減させる」という計算そのものを持つ
クラスで、割引か増額かは渡す値の符号(負なら割引、正なら増額)だけで決まります。

`PercentageDiscountRule`と`PercentageEnhancementRule`は、このクラスに
符号だけ変えて値を渡す薄いラッパーです。計算ロジックの実体は1つしかなく、
「割引の反対を新しい概念として作り直す」のではなく、**同じ仕組みをクラス名だけ
変えて再利用する**という設計にしています。

## なぜCompositeの中の失敗伝播にResultsのBindを使っているか

`CompositePricingRule`は、複数のルールを順番に適用していく際に`Result<Money>.Bind`を
使っています。

```csharp
public Result<Money> Apply(Money price, PricingContext context) =>
    _rules.Aggregate(
        Result<Money>.Success(price),
        (acc, rule) => acc.Bind(currentPrice => rule.Apply(currentPrice, context)));
```

途中のルールが失敗(調整後の金額がマイナスになる、など)すれば、`Bind`が
それ以降のルールの実行を自動的にスキップし、失敗をそのまま呼び出し側まで
伝播させます。`if (result.IsFailure) return ...`をルールの数だけ書く必要はありません。

## Open-Closed原則の実演

`CompositePricingRuleTests.既存コードを変更せずに新しいルールを追加できる`では、
テストコードの中だけで新しいルール(`RoundDownToNearestHundredRule`)を定義し、
既存の`CompositePricingRule`や他のルールを一切変更せずに組み合わせられることを
確認しています。新しい価格調整の仕組みを追加する際、既存コードへの修正が
不要であることをテストとして残しています。

## Moneyがマイナスを許さない理由とResultとの役割分担

`Money`のコンストラクタは、負の金額を渡すと例外を投げます。これは
「そもそも成立しないデータ」を弾くための不変条件です。

一方、`PercentageAdjustmentRule`や`FixedAmountAdjustmentRule`は、調整の結果として
金額がマイナスになりうる場合、`Money`のコンストラクタを負の値で呼び出す前に
自分で検証し、`Result<Money>.Failure`を返します。「割引をかけすぎて金額が
マイナスになった」というのは、起こりうる業務上の失敗であり、例外ではなく
`Results`ライブラリで表現する、という役割分担を徹底しています。