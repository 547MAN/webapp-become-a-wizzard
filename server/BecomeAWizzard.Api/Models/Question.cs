namespace BecomeAWizzard.Api.Models;

public class Question
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    public string Text { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public int Order { get; set; }
    public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
}

