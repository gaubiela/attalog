using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class FinanceInvestmentConfiguration : IEntityTypeConfiguration<FinanceInvestment>
{
    public void Configure(EntityTypeBuilder<FinanceInvestment> builder)
    {
        builder.Property(fi => fi.Amount)
            .HasPrecision(18, 2);

        builder.Property(fi => fi.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(fi => fi.Notes)
            .HasMaxLength(500);

        builder.HasOne(fi => fi.User)
            .WithMany(u => u.FinanceInvestments)
            .HasForeignKey(fi => fi.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
