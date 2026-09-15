namespace BecomeAWizzard.Api.Models;

public class Quiz
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public int StartingHp { get; set; } = 100;
    public int MistakeDamage { get; set; } = 20;
    public bool BossFightEnabled { get; set; }
    public string? BossName { get; set; }
    public int? BossHp { get; set; }
    public int? BossDamagePerStreak { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
}

