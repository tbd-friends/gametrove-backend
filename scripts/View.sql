CREATE VIEW UserSummary AS
SELECT
    COUNT(DISTINCT g.Id) AS Games,
    COUNT(gc.Id) AS Copies,
    COUNT(DISTINCT p.Id) AS Platforms
FROM GameCopies gc
         LEFT JOIN Games g ON gc.GameId = g.Id
         LEFT JOIN Platforms p ON g.PlatformId = p.Id
go

