use gametracking
go

create table dbo.IgdbGameMappings
(
    Id         int identity
        constraint IgdbGameMappings_pk
            primary key,
    GameId     int not null
        constraint IgdbGameMappings_pk_2
            unique,
    IgdbGameId int not null
)
go

create table dbo.IgdbPlatformMappings
(
    Id             int identity
        constraint IgdbPlatformMappings_pk
            primary key,
    PlatformId     int not null,
    IgdbPlatformId int not null
)
go

create index IgdbPlatformMappings_IgdbPlatformId_PlatformId_index
    on dbo.IgdbPlatformMappings (IgdbPlatformId, PlatformId)
go

