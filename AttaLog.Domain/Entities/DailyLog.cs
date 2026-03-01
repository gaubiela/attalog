namespace AttaLog.Domain.Entities;

public class DailyLog : BaseEntity
{
    public Guid GoalId { get; set; }
    public Guid UserId { get; set; }
    public DateOnly Date { get; set; }
    public bool Completed { get; set; }
    public string? Note { get; set; }

    public Goal Goal { get; set; }
    public User User { get; set; }
}
