use gametracking
go

/* Bail out if we don't have the target tables */
IF OBJECT_ID('dbo.PriceChartingGameCopyAssociations', 'U') IS NULL
    OR OBJECT_ID('dbo.PriceChartingHistory', 'U') IS NULL
    BEGIN
        PRINT 'Skipping pricing migration: source tables missing.';
        RETURN;
    END;

/* Bail out if we have the snapshot table already */
IF OBJECT_ID('dbo.PriceChartingSnapshot', 'U') IS NOT NULL
    BEGIN
        PRINT 'Skipping pricing migration: already applied (PriceChartingSnapshot exists).';
        RETURN;
    END;

use gametracking
go

create table dbo.PriceChartingSnapshot
(
    Id                 int identity
        constraint PK_PriceChartingSnapshot
            primary key,
    PriceChartingId    int                                                   not null
        constraint PriceChartingSnapshot_pk
            unique,
    Name               nvarchar(255),
    CompleteInBoxPrice decimal(7, 2)                                         not null,
    LoosePrice         decimal(7, 2)                                         not null,
    NewPrice           decimal(7, 2)
        constraint DF_PriceChartingSnapshot_NewPrice default 0               not null,
    ConsoleName        nvarchar(100),
    LastUpdated        datetime
        constraint DF_PriceChartingSnapshot_LastUpdated default getutcdate() not null
)
go

use gametracking
go

create table dbo.PriceChartingSnapshotHistory
(
    Id                 int identity
        constraint PK_PriceChartingSnapshotHistory
            primary key,
    PriceChartingId    int           not null,
    ImportDate         datetime      not null,
    Name               nvarchar(255) not null,
    CompleteInBoxPrice decimal(7, 2) not null,
    LoosePrice         decimal(7, 2) not null,
    NewPrice           decimal(7, 2) not null,
    ConsoleName        nvarchar(100)
)
go

create index IX_PriceChartingSnapshotHistory_Correlated
    on dbo.PriceChartingSnapshotHistory (PriceChartingId asc, ImportDate desc) include (CompleteInBoxPrice, LoosePrice, NewPrice)
go


create table dbo.GameCopyPricing
(
    Id              int identity
        constraint GameCopyPricing_pk
            primary key,
    GameCopyId      int not null,
    PriceChartingId int not null
)
go

create index GameCopyPricing_PriceChartingId_GameCopyId_index
    on dbo.GameCopyPricing (PriceChartingId, GameCopyId)
go

/*
 Migrate Data from Existing Tables
 */

insert into dbo.PriceChartingSnapshot
(PriceChartingId, Name, CompleteInBoxPrice, LoosePrice, NewPrice, ConsoleName, LastUpdated)
select pcga.PriceChartingId,
       pcga.Name,
       pcga.CompleteInBoxPrice,
       pcga.LoosePrice,
       pcga.NewPrice,
       pcga.ConsoleName,
       pcga.LastUpdated
from PriceChartingGameCopyAssociations pcga
where pcga.Id = (select top 1 pcga2.Id
                 from PriceChartingGameCopyAssociations pcga2
                 where pcga2.PriceChartingId = pcga.PriceChartingId
                 order by pcga2.LastUpdated desc)

insert into dbo.PriceChartingSnapshotHistory
(PriceChartingId, ImportDate, Name, CompleteInBoxPrice, LoosePrice, NewPrice, ConsoleName)
select distinct pcga.PriceChartingId,
                pch.ImportDate,
                pch.Name,
                pch.CompleteInBoxPrice,
                pch.LoosePrice,
                pch.NewPrice,
                pch.ConsoleName
from PriceChartingHistory pch
         join PriceChartingGameCopyAssociations pcga on pch.AssociationId = pcga.Id
where pch.IsCurrent = 0
order by pcga.PriceChartingId, pch.ImportDate DESC

INSERT INTO GameCopyPricing
    (GameCopyId, PriceChartingId)
SELECT DISTINCT GameCopyId, PriceChartingId
FROM PriceChartingGameCopyAssociations

/*
  Drop unnecessary tables
 */

DROP TABLE dbo.PriceChartingGameCopyAssociations
GO

DROP TABLE dbo.PriceChartingHistory
 
 