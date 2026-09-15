using BecomeAWizzard.Api.Data;
using BecomeAWizzard.Api.DTOs;
using BecomeAWizzard.Api.Extensions;
using BecomeAWizzard.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BecomeAWizzard.Api.Controllers;

[Authorize, ApiController, Route("api/progress")]
public class ProgressController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProgressDto>> Get()
    {
        var id = User.GetUserId();
        var user = await db.Users.FindAsync(id);
        var attempts = db.QuizAttempts.Where(a => a.UserId == id);
        var answers = db.AnswerAttempts.Where(a => a.QuizAttempt.UserId == id);
        var answered = await answers.CountAsync(); var correct = await answers.CountAsync(a => a.IsCorrect);
        return Ok(new ProgressDto(user?.TotalXp ?? 0, await attempts.CountAsync(a => a.Status == AttemptStatus.Completed || a.Status == AttemptStatus.Won),
            await attempts.CountAsync(a => a.Status == AttemptStatus.Won), answered, correct, answered == 0 ? 0 : Math.Round(correct * 100d / answered, 1)));
    }

    [HttpGet("history")]
    public async Task<ActionResult> History()
    {
        var attempts = await db.QuizAttempts.AsNoTracking().Include(a => a.Quiz)
            .Where(a => a.UserId == User.GetUserId()).OrderByDescending(a => a.StartedAt).ToListAsync();
        return Ok(attempts.Select(a => new HistoryDto(a.Id, a.Quiz.Title, a.Status.ToString(), a.Score, a.StartedAt, a.CompletedAt)));
    }

    [HttpGet("leaderboard")]
    public async Task<ActionResult> Leaderboard() => Ok(await db.Users.AsNoTracking().OrderByDescending(u => u.TotalXp)
        .Take(20).Select((u) => new { u.Id, u.DisplayName, u.TotalXp }).ToListAsync());
}
