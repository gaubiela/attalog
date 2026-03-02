using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class StreakAchievementConfiguration : IEntityTypeConfiguration<StreakAchievement>
{
    public void Configure(EntityTypeBuilder<StreakAchievement> builder)
    {
        builder.HasIndex(sa => new { sa.UserId, sa.MilestoneId })
            .IsUnique();

        builder.HasOne(sa => sa.User)
            .WithMany(u => u.StreakAchievements)
            .HasForeignKey(sa => sa.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sa => sa.Milestone)
            .WithMany(sm => sm.Achievements)
            .HasForeignKey(sa => sa.MilestoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
