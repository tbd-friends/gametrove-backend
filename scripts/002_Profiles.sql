use gametracking
go

create table dbo.Profiles
(
    Id             int identity
        constraint Profiles_pk
            primary key,
    UserIdentifier nvarchar(255)                 not null,
    Name           nvarchar(255)                 not null,
    FavoriteGame   nvarchar(255),
    LastUpdated    datetime default getutcdate() not null
)
go

