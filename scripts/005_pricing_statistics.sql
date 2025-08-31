use gametracking
go

CREATE VIEW PriceChartingStatistics AS
WITH Latest AS
         (SELECT pcs.Id,
                 pcs.PriceChartingId,
                 pcs.CompleteInBoxPrice,
                 pcs.LoosePrice,
                 pcs.NewPrice,
                 pch.CompleteInBoxPrice AS PreviousCompleteInBoxPrice,
                 pch.LoosePrice AS PreviousLoosePrice,
                 pch.NewPrice AS PreviousNewPrice
          FROM dbo.PriceChartingSnapshot pcs
                   JOIN dbo.PriceChartingSnapshotHistory pch ON pch.PriceChartingId = pcs.PriceChartingId
          WHERE pch.ImportDate =
                (SELECT MAX(pch2.ImportDate)
                 FROM dbo.PriceChartingSnapshotHistory pch2
                 WHERE pch2.PriceChartingId = pch.PriceChartingId)),
     Month12 AS
         (SELECT PriceChartingId,
                 CompleteInBoxPrice AS CIB_12mo,
                 LoosePrice AS Loose_12mo,
                 NewPrice AS New_12mo
          FROM dbo.PriceChartingSnapshotHistory pcsh
          WHERE ImportDate =
                (SELECT MAX(ImportDate)
                 FROM dbo.PriceChartingSnapshotHistory pcsh1
                 WHERE ImportDate <= DATEADD(MONTH, -12, GETDATE()))),
     Stats AS
         (SELECT AVG(IIF(PreviousCompleteInBoxPrice > 0, (CompleteInBoxPrice - PreviousCompleteInBoxPrice) / PreviousCompleteInBoxPrice * 100, 0)) AS MeanCIB,
                 STDEV(IIF(PreviousCompleteInBoxPrice > 0, (CompleteInBoxPrice - PreviousCompleteInBoxPrice) / PreviousCompleteInBoxPrice * 100, 0)) AS StdevCIB,
                 AVG(IIF(PreviousLoosePrice > 0, (LoosePrice - PreviousLoosePrice) / PreviousLoosePrice * 100, 0)) AS MeanLoose,
                 STDEV(IIF(PreviousLoosePrice > 0, (LoosePrice - PreviousLoosePrice) / PreviousLoosePrice * 100, 0)) AS StdevLoose,
                 AVG(IIF(PreviousNewPrice > 0, (NewPrice - PreviousNewPrice) / PreviousNewPrice * 100, 0)) AS MeanNew,
                 STDEV(IIF(PreviousNewPrice > 0, (NewPrice - PreviousNewPrice) / PreviousNewPrice * 100, 0)) AS StdevNew
          FROM Latest),
     Final AS
         (SELECT l.Id,
                 l.PriceChartingId,
                 IIF(l.PreviousCompleteInBoxPrice > 0, (l.CompleteInBoxPrice - l.PreviousCompleteInBoxPrice) / l.PreviousCompleteInBoxPrice * 100, 0) AS CompleteInBoxPC,
                 IIF(m.CIB_12mo > 0, (l.CompleteInBoxPrice - m.CIB_12mo) / m.CIB_12mo * 100, 0) AS CompleteInBoxPC_12mo,
                 IIF(ABS(IIF(l.PreviousCompleteInBoxPrice > 0, (l.CompleteInBoxPrice - l.PreviousCompleteInBoxPrice) / l.PreviousCompleteInBoxPrice * 100, 0) - s.MeanCIB) > 2 * s.StdevCIB, 1, 0) AS IsOutlierCIB,
                 IIF(l.PreviousLoosePrice > 0, (l.LoosePrice - l.PreviousLoosePrice) / l.PreviousLoosePrice * 100, 0) AS LoosePC,
                 IIF(m.Loose_12mo > 0, (l.LoosePrice - m.Loose_12mo) / m.Loose_12mo * 100, 0) AS LoosePC_12mo,
                 IIF(ABS(IIF(l.PreviousLoosePrice > 0, (l.LoosePrice - l.PreviousLoosePrice) / l.PreviousLoosePrice * 100, 0) - s.MeanLoose) > 2 * s.StdevLoose, 1, 0) AS IsOutlierLoose,
                 IIF(l.PreviousNewPrice > 0, (l.NewPrice - l.PreviousNewPrice) / l.PreviousNewPrice * 100, 0) AS NewPC,
                 IIF(m.New_12mo > 0, (l.NewPrice - m.New_12mo) / m.New_12mo * 100, 0) AS NewPC_12mo,
                 IIF(ABS(IIF(l.PreviousNewPrice > 0, (l.NewPrice - l.PreviousNewPrice) / l.PreviousNewPrice * 100, 0) - s.MeanNew) > 2 * s.StdevNew, 1, 0) AS IsOutlierNew
          FROM Latest l
                   LEFT JOIN Month12 m ON l.PriceChartingId = m.PriceChartingId
                   CROSS JOIN Stats s)
SELECT Id, PriceChartingId, CompleteInBoxPC, CompleteInBoxPC_12mo, LoosePC, LoosePC_12mo, NewPC, NewPC_12mo,
       IIF(IsOutlierCIB = 1 OR IsOutlierLoose = 1 OR IsOutlierNew = 1, 1, 0) AS IsOutlier
FROM Final
go

