namespace AttaLog.Domain.Entities;

public class Group : BaseEntity
{
    public string? Name { get; set; }
    public Guid CreatedById { get; set; }

    public User CreatedBy { get; set; }
    public ICollection<GroupMember> Members { get; set; } = [];
    public ICollection<GroupInvite> Invites { get; set; } = [];
    public ICollection<SharedGoalGroup> SharedGoals { get; set; } = [];
    public ICollection<StreakMilestone> StreakMilestones { get; set; } = [];
}
