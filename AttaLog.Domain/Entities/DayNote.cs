namespace AttaLog.Domain.Entities;

public class DayNote : BaseEntity
{
    public Guid UserId { get; set; }
    public DateOnly Date { get; set; }
    public string Content { get; set; }

    public User User { get; set; }
}
