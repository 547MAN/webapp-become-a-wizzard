using BecomeAWizzard.Api.Data;
using BecomeAWizzard.Api.DTOs;
using BecomeAWizzard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BecomeAWizzard.Api.Services;

public class GameService(AppDbContext db)
{
    public async Task<AttemptStateDto> StartAsync(int userId, int quizId)
    {
        var quiz = await LoadQuiz(quizId);
        if (quiz is null || !quiz.IsPublished) throw new KeyNotFoundException("Publisert quiz ble ikke funnet.");
        var attempt = new QuizAttempt { UserId = userId, QuizId = quizId, PlayerHp = quiz.StartingHp, BossHp = quiz.BossFightEnabled ? quiz.BossHp : null };
        db.QuizAttempts.Add(attempt);
        await db.SaveChangesAsync();
        return ToState(attempt, quiz);
    }

    public async Task<AttemptStateDto?> GetStateAsync(int attemptId, int userId)
    {
        var attempt = await db.QuizAttempts.AsNoTracking().SingleOrDefaultAsync(a => a.Id == attemptId && a.UserId == userId);
        if (attempt is null) return null;
        var quiz = await LoadQuiz(attempt.QuizId);
        return quiz is null ? null : ToState(attempt, quiz);
    }

    public async Task<AnswerResultDto> SubmitAnswerAsync(int attemptId, int userId, SubmitAnswerRequest request)
    {
        var attempt = await db.QuizAttempts.SingleOrDefaultAsync(a => a.Id == attemptId && a.UserId == userId)
            ?? throw new KeyNotFoundException("Kampøkten ble ikke funnet.");
        if (attempt.Status != AttemptStatus.Active) throw new InvalidOperationException("Kampøkten er allerede avsluttet.");
        var quiz = await LoadQuiz(attempt.QuizId) ?? throw new KeyNotFoundException("Quizen ble ikke funnet.");
        var expected = quiz.Questions.OrderBy(q => q.Order).ElementAt(attempt.CurrentQuestionIndex % quiz.Questions.Count);
        if (expected.Id != request.QuestionId) throw new InvalidOperationException("Spørsmålet er ikke aktivt.");
        if (await db.AnswerAttempts.AnyAsync(a => a.QuizAttemptId == attemptId && a.QuestionId == request.QuestionId && !attempt.IsBossPhase))
            throw new InvalidOperationException("Spørsmålet er allerede besvart.");

        var chosen = expected.AnswerOptions.SingleOrDefault(a => a.Id == request.AnswerOptionId)
            ?? throw new ArgumentException("Svaralternativet finnes ikke.");
        var correct = expected.AnswerOptions.Single(a => a.IsCorrect);
        var points = chosen.IsCorrect ? 100 : -25;

        if (chosen.IsCorrect)
        {
            attempt.Score += points;
            attempt.CorrectStreak++;
            attempt.HighestStreak = Math.Max(attempt.HighestStreak, attempt.CorrectStreak);
            if (attempt.IsBossPhase && attempt.CorrectStreak >= 3)
            {
                attempt.BossHp = Math.Max(0, (attempt.BossHp ?? 0) - (quiz.BossDamagePerStreak ?? 100));
                attempt.CorrectStreak = 0;
            }
        }
        else
        {
            attempt.Score = Math.Max(0, attempt.Score + points);
            attempt.PlayerHp = Math.Max(0, attempt.PlayerHp - quiz.MistakeDamage);
            attempt.CorrectStreak = 0;
        }

        db.AnswerAttempts.Add(new AnswerAttempt { QuizAttemptId = attempt.Id, QuestionId = expected.Id, SelectedAnswerOptionId = chosen.Id, IsCorrect = chosen.IsCorrect, PointsChange = points });
        attempt.CurrentQuestionIndex++;

        if (attempt.PlayerHp <= 0) Finish(attempt, AttemptStatus.Lost);
        else if (attempt.IsBossPhase && attempt.BossHp <= 0) Finish(attempt, AttemptStatus.Won);
        else if (!attempt.IsBossPhase && attempt.CurrentQuestionIndex >= quiz.Questions.Count)
        {
            if (quiz.BossFightEnabled) { attempt.IsBossPhase = true; attempt.CurrentQuestionIndex = 0; attempt.CorrectStreak = 0; }
            else Finish(attempt, AttemptStatus.Completed);
        }

        if (attempt.Status is AttemptStatus.Won or AttemptStatus.Completed)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is not null) user.TotalXp += attempt.Score;
        }
        await db.SaveChangesAsync();
        return new AnswerResultDto(chosen.IsCorrect, correct.Id, expected.Explanation, attempt.PlayerHp, attempt.BossHp,
            attempt.IsBossPhase, attempt.CorrectStreak, attempt.Score, attempt.Status.ToString());
    }

    private async Task<Quiz?> LoadQuiz(int quizId) => await db.Quizzes.Include(q => q.Questions.OrderBy(x => x.Order))
        .ThenInclude(q => q.AnswerOptions).SingleOrDefaultAsync(q => q.Id == quizId);

    private static AttemptStateDto ToState(QuizAttempt attempt, Quiz quiz)
    {
        PlayQuestionDto? question = null;
        if (attempt.Status == AttemptStatus.Active)
        {
            var current = quiz.Questions.OrderBy(q => q.Order).ElementAt(attempt.CurrentQuestionIndex % quiz.Questions.Count);
            question = new PlayQuestionDto(current.Id, current.Text, current.AnswerOptions.Select(a => new AnswerOptionDto(a.Id, a.Text)).ToList());
        }
        return new AttemptStateDto(attempt.Id, quiz.Title, attempt.PlayerHp, quiz.StartingHp, attempt.BossHp, quiz.BossHp, attempt.IsBossPhase,
            attempt.CorrectStreak, attempt.Score, attempt.Status.ToString(), question);
    }

    private static void Finish(QuizAttempt attempt, AttemptStatus status) { attempt.Status = status; attempt.CompletedAt = DateTime.UtcNow; }
}
