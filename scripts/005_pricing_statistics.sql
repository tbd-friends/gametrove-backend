use gametracking
go

CREATE view dbo.PriceChartingStatistics as
WITH Latest AS (SELECT pcs.Id,
                       pcs.PriceChartingId,
                       pcs.CompleteInBoxPrice,
                       pcs.LoosePrice,
                       pcs.NewPrice,
                       pch.CompleteInBoxPrice AS PreviousCompleteInBoxPrice,
                       pch.LoosePrice         AS PreviousLoosePrice,
                       pch.NewPrice           AS PreviousNewPrice
                FROM dbo.PriceChartingSnapshot pcs
                         JOIN dbo.PriceChartingSnapshotHistory pch ON pch.PriceChartingId = pcs.PriceChartingId
                WHERE pch.ImportDate = (SELECT MAX(pch2.ImportDate)
                                        FROM dbo.PriceChartingSnapshotHistory pch2
                                        WHERE pch2.PriceChartingId = pcs.PriceChartingId)),
     Month12 AS (SELECT PriceChartingId,
                        CompleteInBoxPrice AS CIB_12mo,
                        LoosePrice         AS Loose_12mo,
                        NewPrice           AS New_12mo
                 FROM dbo.PriceChartingSnapshotHistory pcsh
                 WHERE ImportDate = (SELECT MAX(ImportDate)
                                     FROM dbo.PriceChartingSnapshotHistory pcsh1
                                     WHERE ImportDate <= DATEADD(MONTH, -12, GETDATE())
                                       AND pcsh1.PriceChartingId = pcsh.PriceChartingId)),
     Stats AS (SELECT AVG(CASE
                              WHEN PreviousCompleteInBoxPrice > 0 THEN
                                  (CompleteInBoxPrice - PreviousCompleteInBoxPrice) / PreviousCompleteInBoxPrice *
                                  100
                              ELSE 0 END)   AS MeanCIB,
                      STDEV(CASE
                                WHEN PreviousCompleteInBoxPrice > 0 THEN
                                    (CompleteInBoxPrice - PreviousCompleteInBoxPrice) / PreviousCompleteInBoxPrice *
                                    100
                                ELSE 0 END) AS StdevCIB,
                      AVG(CASE
                              WHEN PreviousLoosePrice > 0
                                  THEN (LoosePrice - PreviousLoosePrice) / PreviousLoosePrice * 100
                              ELSE 0 END)   AS MeanLoose,
                      STDEV(CASE
                                WHEN PreviousLoosePrice > 0
                                    THEN (LoosePrice - PreviousLoosePrice) / PreviousLoosePrice * 100
                                ELSE 0 END) AS StdevLoose,
                      AVG(CASE
                              WHEN PreviousNewPrice > 0 THEN (NewPrice - PreviousNewPrice) / PreviousNewPrice * 100
                              ELSE 0 END)   AS MeanNew,
                      STDEV(CASE
                                WHEN PreviousNewPrice > 0
                                    THEN (NewPrice - PreviousNewPrice) / PreviousNewPrice * 100
                                ELSE 0 END) AS StdevNew
               FROM Latest),
     Final AS (SELECT l.Id,
                      l.PriceChartingId,
                      CASE
                          WHEN l.PreviousCompleteInBoxPrice > 0 THEN
                              (l.CompleteInBoxPrice - l.PreviousCompleteInBoxPrice) / l.PreviousCompleteInBoxPrice *
                              100
                          ELSE 0 END                                                                        AS CompleteInBoxPC,
                      CASE
                          WHEN m.CIB_12mo > 0 THEN (l.CompleteInBoxPrice - m.CIB_12mo) / m.CIB_12mo * 100
                          ELSE 0 END                                                                        AS CompleteInBoxPC_12mo,
                      CASE
                          WHEN ABS(CASE
                                       WHEN l.PreviousCompleteInBoxPrice > 0 THEN
                                           (l.CompleteInBoxPrice - l.PreviousCompleteInBoxPrice) /
                                           l.PreviousCompleteInBoxPrice * 100
                                       ELSE 0 END - s.MeanCIB) > 2 * s.StdevCIB THEN 1
                          ELSE 0 END                                                                        AS IsOutlierCIB,
                      CASE
                          WHEN l.PreviousLoosePrice > 0
                              THEN (l.LoosePrice - l.PreviousLoosePrice) / l.PreviousLoosePrice * 100
                          ELSE 0 END                                                                        AS LoosePC,
                      CASE
                          WHEN m.Loose_12mo > 0 THEN (l.LoosePrice - m.Loose_12mo) / m.Loose_12mo * 100
                          ELSE 0 END                                                                        AS LoosePC_12mo,
                      CASE
                          WHEN ABS(CASE
                                       WHEN l.PreviousLoosePrice > 0
                                           THEN (l.LoosePrice - l.PreviousLoosePrice) / l.PreviousLoosePrice * 100
                                       ELSE 0 END - s.MeanLoose) > 2 * s.StdevLoose THEN 1
                          ELSE 0 END                                                                        AS IsOutlierLoose,
                      CASE
                          WHEN l.PreviousNewPrice > 0
                              THEN (l.NewPrice - l.PreviousNewPrice) / l.PreviousNewPrice * 100
                          ELSE 0 END                                                                        AS NewPC,
                      CASE
                          WHEN m.New_12mo > 0 THEN (l.NewPrice - m.New_12mo) / m.New_12mo * 100
                          ELSE 0 END                                                                        AS NewPC_12mo,
                      CASE
                          WHEN ABS(CASE
                                       WHEN l.PreviousNewPrice > 0
                                           THEN (l.NewPrice - l.PreviousNewPrice) / l.PreviousNewPrice * 100
                                       ELSE 0 END - s.MeanNew) > 2 * s.StdevNew THEN 1
                          ELSE 0 END                                                                        AS IsOutlierNew
               FROM Latest l
                        LEFT JOIN Month12 m ON l.PriceChartingId = m.PriceChartingId
                        CROSS JOIN Stats s)
SELECT Id,
       PriceChartingId,
       CompleteInBoxPC,
       CompleteInBoxPC_12mo,
       LoosePC,
       LoosePC_12mo,
       NewPC,
       NewPC_12mo,
       CASE WHEN IsOutlierCIB = 1 OR IsOutlierLoose = 1 OR IsOutlierNew = 1 THEN 1 ELSE 0 END AS IsOutlier
FROM Final
go

