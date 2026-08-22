# SampleApp

`BuildingBlocks` / `Results` / `Pricing` の3つを組み合わせた最小限のコンソールアプリです。

「入力検証 → ドメインオブジェクト生成 → 料金計算」という、実務でよくある業務フローを
1本の流れとして表現しています。

## フロー

```
文字列の入力(単価・数量)
    │  OrderInputParser (Results: Result<T>で検証)
    ▼
検証済みの値
    │  Order.Create (BuildingBlocks: Entity<Guid>を生成。Resultで数量を検証)
    ▼
Orderエンティティ
    │  ApplyPricingRules (Pricing: CompositePricingRuleで割引を適用)
    ▼
最終的な合計金額 (Result<Money>)
```

途中のどこかで検証に失敗すれば(単価が数値として解釈できない、数量が0以下など)、
`Bind`によってそれ以降の処理は実行されず、失敗がそのまま最後まで伝播します。
`try-catch`は一切使っていません。

## 実行方法

```bash
dotnet run --project samples/SampleApp
```

`Program.cs`内の`rawUnitPrice` / `rawQuantity` / `isMember`の値を変えると、
異なる結果(成功・失敗、適用される割引の内容)を確認できます。