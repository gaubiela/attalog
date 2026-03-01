namespace AttaLog.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public ICollection<GroupMember> GroupMembers { get; set; } = [];
    public ICollection<Goal> Goals { get; set; } = [];
    public ICollection<DailyLog> DailyLogs { get; set; } = [];
    public ICollection<DayNote> DayNotes { get; set; } = [];
    public ICollection<StreakAchievement> StreakAchievements { get; set; } = [];
    public ICollection<FinanceCategory> FinanceCategories { get; set; } = [];
    public ICollection<FinanceTransaction> FinanceTransactions { get; set; } = [];
    public ICollection<FinanceInvestment> FinanceInvestments { get; set; } = [];
}
