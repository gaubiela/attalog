using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class StreakMilestoneConfiguration : IEntityTypeConfiguration<StreakMilestone>
{
    public void Configure(EntityTypeBuilder<StreakMilestone> builder)
    {
        builder.Property(sm => sm.Reward)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(sm => sm.Group)
            .WithMany(g => g.StreakMilestones)
            .HasForeignKey(sm => sm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sm => sm.CreatedByUser)
            .WithMany()
            .HasForeignKey(sm => sm.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
