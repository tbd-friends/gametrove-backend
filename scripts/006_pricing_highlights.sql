use gametracking
go

CREATE VIEW PriceChartingHighlights AS
WITH Highlights AS (
    SELECT
        pcs.PriceChartingId,
        GREATEST(
                ABS(pcs.CompleteInBoxPC),
                ABS(pcs.LoosePC),
                ABS(pcs.NewPC)
        ) AS DifferencePercentage
    FROM PriceChartingStatistics pcs
    WHERE IsOutlier = 1
), GameByPriceChartingId AS (
    SELECT DISTINCT g.Identifier, g.Name, gcp.PriceChartingId
    FROM Games g
             JOIN GameCopies gc ON g.Id = gc.GameId
             JOIN GameCopyPricing gcp ON gc.Id = gcp.GameCopyId
)

SELECT gbpc.Identifier AS GameIdentifier, gbpc.Name, h.*
FROM Highlights h
         JOIN GameByPriceChartingId gbpc ON h.PriceChartingId = gbpc.PriceChartingId
go

