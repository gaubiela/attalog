using System.Reflection;
using AttaLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttaLog.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupInvite> GroupInvites { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<SharedGoalGroup> SharedGoalGroups { get; set; }
    public DbSet<DailyLog> DailyLogs { get; set; }
    public DbSet<DayNote> DayNotes { get; set; }
    public DbSet<StreakMilestone> StreakMilestones { get; set; }
    public DbSet<StreakAchievement> StreakAchievements { get; set; }
    public DbSet<FinanceCategory> FinanceCategories { get; set; }
    public DbSet<FinanceTransaction> FinanceTransactions { get; set; }
    public DbSet<FinanceInvestment> FinanceInvestments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
