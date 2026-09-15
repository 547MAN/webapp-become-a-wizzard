using System.ComponentModel.DataAnnotations;

namespace BecomeAWizzard.Api.DTOs;

public record AnswerOptionInput([Required] string Text, bool IsCorrect);
public record QuestionInput([Required] string Text, [Required] string Explanation, int Order, List<AnswerOptionInput> AnswerOptions);

public record QuizInput(
    [Required, MaxLength(100)] string Title,
    [Required, MaxLength(500)] string Description,
    [Required, MaxLength(80)] string Topic,
    bool IsPublished,
    int StartingHp,
    int MistakeDamage,
    bool BossFightEnabled,
    string? BossName,
    int? BossHp,
    int? BossDamagePerStreak,
    List<QuestionInput> Questions);

public record AnswerOptionDto(int Id, string Text);
public record AnswerOptionEditDto(int Id, string Text, bool IsCorrect);
public record QuestionEditDto(int Id, string Text, string Explanation, int Order, IReadOnlyList<AnswerOptionEditDto> AnswerOptions);
public record QuizSummaryDto(int Id, string Title, string Description, string Topic, string OwnerName, bool IsPublished, bool BossFightEnabled, int QuestionCount);
public record QuizDetailsDto(int Id, int OwnerId, string Title, string Description, string Topic, bool IsPublished, int StartingHp, int MistakeDamage, bool BossFightEnabled, string? BossName, int? BossHp, int? BossDamagePerStreak, IReadOnlyList<QuestionEditDto> Questions);
