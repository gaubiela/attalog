namespace AttaLog.Domain.Entities;

public class FinanceInvestment : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateOnly InvestmentDate { get; set; }
    public string? Notes { get; set; }

    public User User { get; set; }
}
