using BecomeAWizzard.Api.Data;
using BecomeAWizzard.Api.DTOs;
using BecomeAWizzard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BecomeAWizzard.Api.Services;

public class QuizService(AppDbContext db)
{
    public async Task<IReadOnlyList<QuizSummaryDto>> GetPublishedAsync() =>
        await db.Quizzes.AsNoTracking().Where(q => q.IsPublished)
            .OrderByDescending(q => q.UpdatedAt)
            .Select(q => new QuizSummaryDto(q.Id, q.Title, q.Description, q.Topic, q.Owner.DisplayName, q.IsPublished, q.BossFightEnabled, q.Questions.Count))
            .ToListAsync();

    public async Task<IReadOnlyList<QuizSummaryDto>> GetMineAsync(int userId) =>
        await db.Quizzes.AsNoTracking().Where(q => q.OwnerId == userId)
            .OrderByDescending(q => q.UpdatedAt)
            .Select(q => new QuizSummaryDto(q.Id, q.Title, q.Description, q.Topic, q.Owner.DisplayName, q.IsPublished, q.BossFightEnabled, q.Questions.Count))
            .ToListAsync();

    public async Task<QuizDetailsDto?> GetAsync(int id, int userId)
    {
        var quiz = await FullQuiz().AsNoTracking().SingleOrDefaultAsync(q => q.Id == id);
        // Full quiz details contain the answer key and are therefore available only to the owner.
        if (quiz is null || quiz.OwnerId != userId) return null;
        return ToDetails(quiz);
    }

    public async Task<QuizDetailsDto> CreateAsync(int userId, QuizInput input)
    {
        Validate(input);
        var quiz = new Quiz { OwnerId = userId };
        Apply(quiz, input);
        db.Quizzes.Add(quiz);
        await db.SaveChangesAsync();
        return ToDetails(quiz);
    }

    public async Task<QuizDetailsDto?> UpdateAsync(int id, int userId, QuizInput input)
    {
        Validate(input);
        var quiz = await FullQuiz().SingleOrDefaultAsync(q => q.Id == id && q.OwnerId == userId);
        if (quiz is null) return null;
        db.AnswerOptions.RemoveRange(quiz.Questions.SelectMany(q => q.AnswerOptions));
        db.Questions.RemoveRange(quiz.Questions);
        quiz.Questions.Clear();
        Apply(quiz, input);
        await db.SaveChangesAsync();
        return ToDetails(quiz);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var quiz = await db.Quizzes.Include(q => q.Attempts).SingleOrDefaultAsync(q => q.Id == id && q.OwnerId == userId);
        if (quiz is null) return false;
        if (quiz.Attempts.Count > 0)
        {
            quiz.IsPublished = false;
            if (!quiz.Title.EndsWith(" (arkivert)")) quiz.Title += " (arkivert)";
        }
        else db.Quizzes.Remove(quiz);
        await db.SaveChangesAsync();
        return true;
    }

    private IQueryable<Quiz> FullQuiz() => db.Quizzes.Include(q => q.Owner)
        .Include(q => q.Questions.OrderBy(question => question.Order))
        .ThenInclude(q => q.AnswerOptions);

    private static void Validate(QuizInput input)
    {
        if (input.StartingHp <= 0 || input.MistakeDamage <= 0)
            throw new ArgumentException("HP og skade må være større enn null.");
        if (input.Questions.Count == 0)
            throw new ArgumentException("Quizen må inneholde minst ett spørsmål.");
        if (input.IsPublished && input.Questions.Count < 3)
            throw new ArgumentException("En publisert quiz må inneholde minst tre spørsmål.");
        foreach (var question in input.Questions)
        {
            if (question.AnswerOptions.Count < 2 || question.AnswerOptions.Count(option => option.IsCorrect) != 1)
                throw new ArgumentException("Hvert spørsmål må ha minst to alternativer og nøyaktig ett riktig svar.");
        }
        if (input.BossFightEnabled && (string.IsNullOrWhiteSpace(input.BossName) || input.BossHp <= 0 || input.BossDamagePerStreak <= 0))
            throw new ArgumentException("Bossnavn, boss-HP og boss-skade kreves når bosskamp er aktivert.");
    }

    private static void Apply(Quiz quiz, QuizInput input)
    {
        quiz.Title = input.Title.Trim(); quiz.Description = input.Description.Trim(); quiz.Topic = input.Topic.Trim();
        quiz.IsPublished = input.IsPublished; quiz.StartingHp = input.StartingHp; quiz.MistakeDamage = input.MistakeDamage;
        quiz.BossFightEnabled = input.BossFightEnabled; quiz.BossName = input.BossFightEnabled ? input.BossName?.Trim() : null;
        quiz.BossHp = input.BossFightEnabled ? input.BossHp : null; quiz.BossDamagePerStreak = input.BossFightEnabled ? input.BossDamagePerStreak : null;
        quiz.UpdatedAt = DateTime.UtcNow;
        quiz.Questions = input.Questions.Select((question, index) => new Question
        {
            Text = question.Text.Trim(), Explanation = question.Explanation.Trim(), Order = index + 1,
            AnswerOptions = question.AnswerOptions.Select(option => new AnswerOption { Text = option.Text.Trim(), IsCorrect = option.IsCorrect }).ToList()
        }).ToList();
    }

    private static QuizDetailsDto ToDetails(Quiz q) => new(q.Id, q.OwnerId, q.Title, q.Description, q.Topic, q.IsPublished,
        q.StartingHp, q.MistakeDamage, q.BossFightEnabled, q.BossName, q.BossHp, q.BossDamagePerStreak,
        q.Questions.OrderBy(x => x.Order).Select(question => new QuestionEditDto(question.Id, question.Text, question.Explanation, question.Order,
            question.AnswerOptions.Select(option => new AnswerOptionEditDto(option.Id, option.Text, option.IsCorrect)).ToList())).ToList());
}
