using AttaLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttaLog.Infrastructure.Data.Configurations;

public class FinanceTransactionConfiguration : IEntityTypeConfiguration<FinanceTransaction>
{
    public void Configure(EntityTypeBuilder<FinanceTransaction> builder)
    {
        builder.Property(ft => ft.Amount)
            .HasPrecision(18, 2);

        builder.Property(ft => ft.Description)
            .HasMaxLength(300);

        builder.Property(ft => ft.Merchant)
            .HasMaxLength(200);

        builder.Property(ft => ft.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(ft => !ft.IsDeleted);

        builder.HasOne(ft => ft.User)
            .WithMany(u => u.FinanceTransactions)
            .HasForeignKey(ft => ft.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ft => ft.Category)
            .WithMany(fc => fc.Transactions)
            .HasForeignKey(ft => ft.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
