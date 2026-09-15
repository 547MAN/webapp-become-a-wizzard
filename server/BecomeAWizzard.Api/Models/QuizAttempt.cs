namespace BecomeAWizzard.Api.Models;

public enum AttemptStatus { Active, Won, Lost, Completed, Abandoned }

public class QuizAttempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    public int CurrentQuestionIndex { get; set; }
    public int PlayerHp { get; set; }
    public int? BossHp { get; set; }
    public int CorrectStreak { get; set; }
    public int HighestStreak { get; set; }
    public int Score { get; set; }
    public bool IsBossPhase { get; set; }
    public AttemptStatus Status { get; set; } = AttemptStatus.Active;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public ICollection<AnswerAttempt> Answers { get; set; } = new List<AnswerAttempt>();
}

