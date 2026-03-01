namespace AttaLog.Domain.Entities;

public class StreakAchievement : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid MilestoneId { get; set; }
    public DateTime AchievedAt { get; set; }
    public int StreakCountAtTime { get; set; }

    public User User { get; set; }
    public StreakMilestone Milestone { get; set; }
}
