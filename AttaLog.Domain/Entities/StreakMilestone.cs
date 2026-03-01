namespace AttaLog.Domain.Entities;

public class StreakMilestone : BaseEntity
{
    public Guid GroupId { get; set; }
    public int DaysRequired { get; set; }
    public string Reward { get; set; }
    public Guid CreatedByUserId { get; set; }

    public Group Group { get; set; }
    public User CreatedByUser { get; set; }
    public ICollection<StreakAchievement> Achievements { get; set; } = [];
}
