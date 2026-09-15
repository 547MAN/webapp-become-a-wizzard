namespace BecomeAWizzard.Api.Models;

public class AnswerAttempt
{
    public int Id { get; set; }
    public int QuizAttemptId { get; set; }
    public QuizAttempt QuizAttempt { get; set; } = null!;
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public int SelectedAnswerOptionId { get; set; }
    public bool IsCorrect { get; set; }
    public int PointsChange { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}

