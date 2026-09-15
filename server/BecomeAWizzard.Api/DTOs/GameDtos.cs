namespace BecomeAWizzard.Api.DTOs;

public record StartAttemptRequest(int QuizId);
public record SubmitAnswerRequest(int QuestionId, int AnswerOptionId);
public record PlayQuestionDto(int Id, string Text, IReadOnlyList<AnswerOptionDto> AnswerOptions);
public record AttemptStateDto(int AttemptId, string QuizTitle, int PlayerHp, int PlayerMaxHp, int? BossHp, int? BossMaxHp, bool IsBossPhase, int CorrectStreak, int Score, string Status, PlayQuestionDto? Question);
public record AnswerResultDto(bool IsCorrect, int CorrectAnswerOptionId, string Explanation, int PlayerHp, int? BossHp, bool IsBossPhase, int CorrectStreak, int Score, string Status);
public record ProgressDto(int TotalXp, int QuizzesCompleted, int BossesDefeated, int QuestionsAnswered, int CorrectAnswers, double Accuracy);
public record HistoryDto(int AttemptId, string QuizTitle, string Status, int Score, DateTime StartedAt, DateTime? CompletedAt);
