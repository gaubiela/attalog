namespace AttaLog.Domain.Entities;

public class SharedGoalGroup
{
    public Guid GoalId { get; set; }
    public Guid GroupId { get; set; }
    public DateTime SharedAt { get; set; }

    public Goal Goal { get; set; }
    public Group Group { get; set; }
}
