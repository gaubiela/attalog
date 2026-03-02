using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class GroupInviteConfiguration : IEntityTypeConfiguration<GroupInvite>
{
    public void Configure(EntityTypeBuilder<GroupInvite> builder)
    {
        builder.Property(gi => gi.Token)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(gi => gi.Token)
            .IsUnique();

        builder.Property(gi => gi.InvitedEmail)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(gi => gi.ExpiresAt)
            .IsRequired();

        builder.Property(gi => gi.Status)
            .HasDefaultValue(0);

        builder.HasOne(gi => gi.Group)
            .WithMany(g => g.Invites)
            .HasForeignKey(gi => gi.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gi => gi.InvitedBy)
            .WithMany()
            .HasForeignKey(gi => gi.InvitedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
