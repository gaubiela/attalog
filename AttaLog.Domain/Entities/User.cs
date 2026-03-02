using Microsoft.AspNetCore.Identity;

namespace AttaLog.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GroupMember> GroupMembers { get; set; } = [];
    public ICollection<Goal> Goals { get; set; } = [];
    public ICollection<DailyLog> DailyLogs { get; set; } = [];
    public ICollection<DayNote> DayNotes { get; set; } = [];
    public ICollection<StreakAchievement> StreakAchievements { get; set; } = [];
    public ICollection<FinanceCategory> FinanceCategories { get; set; } = [];
    public ICollection<FinanceTransaction> FinanceTransactions { get; set; } = [];
    public ICollection<FinanceInvestment> FinanceInvestments { get; set; } = [];
}
