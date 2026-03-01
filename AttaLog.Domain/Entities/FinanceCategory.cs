using AttaLog.Domain.Enums;

namespace AttaLog.Domain.Entities;

public class FinanceCategory : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string? Icon { get; set; }
    public TransactionType Type { get; set; }
    public bool IsDeleted { get; set; }

    public User User { get; set; }
    public ICollection<FinanceTransaction> Transactions { get; set; } = [];
}
