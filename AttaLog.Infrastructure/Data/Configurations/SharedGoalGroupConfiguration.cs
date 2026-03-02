using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class SharedGoalGroupConfiguration : IEntityTypeConfiguration<SharedGoalGroup>
{
    public void Configure(EntityTypeBuilder<SharedGoalGroup> builder)
    {
        builder.HasKey(sg => new { sg.GoalId, sg.GroupId });

        builder.HasOne(sg => sg.Goal)
            .WithMany(g => g.SharedGoalGroups)
            .HasForeignKey(sg => sg.GoalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sg => sg.Group)
            .WithMany(g => g.SharedGoals)
            .HasForeignKey(sg => sg.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
