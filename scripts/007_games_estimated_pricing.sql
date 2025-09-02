use gametracking
go

CREATE VIEW GamesEstimatedPricing AS
    WITH CopiesWithCondition AS (SELECT gc.Id,
                                    g.Id                 AS GameId,
                                    g.Name,
                                    gcp.PriceChartingId,
                                    CASE
                                        WHEN (Condition & 256) = 256 THEN 'NEW'
                                        WHEN (Condition & 32) = 32 AND (Condition & 256) = 0 THEN 'COMPLETE'
                                        WHEN (Condition & 128) = 128 AND (Condition & 32) = 0 THEN 'LOOSE'
                                        ELSE 'Other' END AS EstimatedCondition
                             FROM GameCopies gc
                                      JOIN Games g ON gc.GameId = g.Id
                                      JOIN GameCopyPricing gcp ON gc.Id = gcp.GameCopyId),
    EstimatedPricing AS (SELECT      c.Id    AS GameCopyId,
                                c.GameId,
                                c.Name,
                                c.EstimatedCondition,
                                pcs.ConsoleName,
                                pcs.PriceChartingId,
     CASE
    WHEN c.EstimatedCondition = 'NEW' THEN pcs.NewPrice
    WHEN c.EstimatedCondition = 'COMPLETE' THEN pcs.CompleteInBoxPrice
    WHEN c.EstimatedCondition = 'LOOSE' THEN pcs.LoosePrice
    END AS Price
                         FROM CopiesWithCondition c
                                  JOIN PriceChartingSnapshot pcs ON c.PriceChartingId = pcs.PriceChartingId
)
SELECT *
FROM EstimatedPricing
go

