using AttaLog.Domain.Enums;

namespace AttaLog.Domain.Entities;

public class Goal : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public GoalType Type { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsDeleted { get; set; }

    public User User { get; set; }
    public ICollection<DailyLog> DailyLogs { get; set; } = [];
    public ICollection<SharedGoalGroup> SharedGoalGroups { get; set; } = [];
}
