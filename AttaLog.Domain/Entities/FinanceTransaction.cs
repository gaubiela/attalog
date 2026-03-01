using AttaLog.Domain.Enums;

namespace AttaLog.Domain.Entities;

public class FinanceTransaction : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Merchant { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateOnly TransactionDate { get; set; }
    public bool IsInstallment { get; set; }
    public int? InstallmentCount { get; set; }
    public int? InstallmentCurrent { get; set; }
    public Guid? InstallmentGroupId { get; set; }
    public bool IsDeleted { get; set; }

    public User User { get; set; }
    public FinanceCategory Category { get; set; }
}
