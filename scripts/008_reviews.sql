use gametracking
go

create table dbo.Reviews
(
    Id            int identity
        constraint Reviews_pk
            primary key,
    GameId        int                           not null,
    Title         nvarchar(255)                 not null,
    Content       nvarchar(max)                 not null,
    Graphics      smallint default 1            not null,
    Sound         smallint default 1            not null,
    Gameplay      smallint default 1            not null,
    Replayability smallint default 1            not null,
    OverallRating smallint default 0            not null,
    Completed     bit                           not null,
    DateAdded     datetime default getutcdate() not null,
    LastModified  datetime
)
go

create index Reviews_GameId_index
    on dbo.Reviews (GameId)
go

