use gametracking
go

CREATE FUNCTION dbo.GetSoundexName
(
    @Name NVARCHAR(255)
)
    RETURNS NVARCHAR(255)
AS
BEGIN
    RETURN SOUNDEX(TRIM(REPLACE(REPLACE(REPLACE(@Name, 'The', ''), 'And', ''), ':','')))
end
go

