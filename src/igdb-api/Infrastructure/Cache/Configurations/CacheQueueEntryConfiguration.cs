using igdb_api.Infrastructure.Cache.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace igdb_api.Infrastructure.Cache.Configurations;

public class CacheQueueEntryConfiguration : IEntityTypeConfiguration<CacheQueueEntry>
{
    public void Configure(EntityTypeBuilder<CacheQueueEntry> builder)
    {
        builder.ToCollection("cache-queue-entries");
    }
}