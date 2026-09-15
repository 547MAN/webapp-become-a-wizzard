using BecomeAWizzard.Api.DTOs;
using BecomeAWizzard.Api.Extensions;
using BecomeAWizzard.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BecomeAWizzard.Api.Controllers;

[Authorize, ApiController, Route("api/quizzes")]
public class QuizzesController(QuizService quizService) : ControllerBase
{
    [HttpGet] public async Task<ActionResult> Published() => Ok(await quizService.GetPublishedAsync());
    [HttpGet("mine")] public async Task<ActionResult> Mine() => Ok(await quizService.GetMineAsync(User.GetUserId()));
    [HttpGet("{id:int}")] public async Task<ActionResult> Get(int id) => await quizService.GetAsync(id, User.GetUserId()) is { } quiz ? Ok(quiz) : NotFound();

    [HttpPost]
    public async Task<ActionResult> Create(QuizInput input)
    {
        try { var quiz = await quizService.CreateAsync(User.GetUserId(), input); return CreatedAtAction(nameof(Get), new { id = quiz.Id }, quiz); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, QuizInput input)
    {
        try { return await quizService.UpdateAsync(id, User.GetUserId(), input) is { } quiz ? Ok(quiz) : NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => await quizService.DeleteAsync(id, User.GetUserId()) ? NoContent() : NotFound();
}

