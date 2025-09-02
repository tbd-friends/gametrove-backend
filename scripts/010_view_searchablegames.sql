use gametracking
go

CREATE VIEW SearchableGames AS
SELECT g.Id, g.Identifier, g.Name, p.Name AS Platform, dbo.GetSoundexName(g.Name) AS SoundexName
FROM Games g
         JOIN Platforms p ON g.PlatformId = p.Id
WHERE EXISTS (
    SELECT *
    FROM GameCopies gc
    WHERE gc.GameId = g.Id
)
go

