using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class DayNoteConfiguration : IEntityTypeConfiguration<DayNote>
{
    public void Configure(EntityTypeBuilder<DayNote> builder)
    {
        builder.HasIndex(dn => new { dn.UserId, dn.Date })
            .IsUnique();

        builder.Property(dn => dn.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(dn => dn.User)
            .WithMany(u => u.DayNotes)
            .HasForeignKey(dn => dn.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
