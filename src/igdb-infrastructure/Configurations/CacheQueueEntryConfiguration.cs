using igdb_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.EntityFrameworkCore.Extensions;

namespace igdb_infrastructure.Configurations;

public class CacheQueueEntryConfiguration : IEntityTypeConfiguration<CacheQueueEntry>
{
    
    public void Configure(EntityTypeBuilder<CacheQueueEntry> builder)
    {
        BsonClassMap.RegisterClassMap<CacheQueueEntryMap>();

        builder.ToCollection("cache-queue-entries");
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public sealed class CacheQueueEntryMap : BsonClassMap<CacheQueueEntry>
    {
        public CacheQueueEntryMap()
        {
            AutoMap();

            MapIdMember(x => x.Id);
        }
    }
}