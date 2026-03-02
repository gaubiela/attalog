using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class DailyLogConfiguration : IEntityTypeConfiguration<DailyLog>
{
    public void Configure(EntityTypeBuilder<DailyLog> builder)
    {
        builder.HasIndex(dl => new { dl.GoalId, dl.UserId, dl.Date })
            .IsUnique();

        builder.Property(dl => dl.Completed)
            .HasDefaultValue(false);

        builder.HasOne(dl => dl.Goal)
            .WithMany(g => g.DailyLogs)
            .HasForeignKey(dl => dl.GoalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(dl => dl.User)
            .WithMany(u => u.DailyLogs)
            .HasForeignKey(dl => dl.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(d => !d.Goal.IsDeleted);
    }
}
