using igdb_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.EntityFrameworkCore.Extensions;

namespace igdb_infrastructure.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToCollection("games");
    }
    
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public sealed class GameEntryMap : BsonClassMap<CacheQueueEntry>
    {
        public GameEntryMap()
        {
            AutoMap();

            MapIdMember(x => x.Id);
        }
    }
}