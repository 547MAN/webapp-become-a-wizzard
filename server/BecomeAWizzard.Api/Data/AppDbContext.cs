using BecomeAWizzard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BecomeAWizzard.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
    public DbSet<AnswerAttempt> AnswerAttempts => Set<AnswerAttempt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
        modelBuilder.Entity<Quiz>().HasOne(q => q.Owner).WithMany(u => u.Quizzes)
            .HasForeignKey(q => q.OwnerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<QuizAttempt>().HasOne(a => a.User).WithMany(u => u.QuizAttempts)
            .HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<QuizAttempt>().Property(a => a.Status).HasConversion<string>();
        modelBuilder.Entity<Question>().HasMany(q => q.AnswerOptions).WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId).OnDelete(DeleteBehavior.Cascade);
    }
}

