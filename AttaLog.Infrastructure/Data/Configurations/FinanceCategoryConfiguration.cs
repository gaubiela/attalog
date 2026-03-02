using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class FinanceCategoryConfiguration : IEntityTypeConfiguration<FinanceCategory>
{
    public void Configure(EntityTypeBuilder<FinanceCategory> builder)
    {
        builder.Property(fc => fc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(fc => fc.Color)
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(fc => fc.Icon)
            .HasMaxLength(50);

        builder.Property(fc => fc.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(fc => !fc.IsDeleted);

        builder.HasOne(fc => fc.User)
            .WithMany(u => u.FinanceCategories)
            .HasForeignKey(fc => fc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
