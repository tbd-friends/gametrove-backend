using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class UserSummaryViewConfiguration : IEntityTypeConfiguration<UserSummary>
{
    public void Configure(EntityTypeBuilder<UserSummary> builder)
    {
        builder.ToView("UserSummary");
    }
}