use gametracking
go

CREATE VIEW PriceChartingGameAverageChanges WITH SCHEMABINDING AS
(
SELECT g.Id,
       AVG(pcs.CompleteInBoxPC) AS AvgCompleteInBoxPriceDiff,
       AVG(pcs.LoosePC)         AS AvgLoosePriceDiff,
       AVG(pcs.NewPC)           AS AvgNewPriceDiff
FROM dbo.Games g
         JOIN dbo.GameCopies gc ON g.Id = gc.GameId
         JOIN dbo.GameCopyPricing gcp ON gc.Id = gcp.GameCopyId
         JOIN dbo.PriceChartingStatistics pcs ON gcp.PriceChartingId = pcs.PriceChartingId
GROUP BY g.Id, pcs.PriceChartingId
    )
go

