namespace AttaLog.Application.DTOs.Group;

public class GroupResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GroupMemberResponse> Members { get; set; } = [];
}
