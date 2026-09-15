using BecomeAWizzard.Api.DTOs;
using BecomeAWizzard.Api.Extensions;
using BecomeAWizzard.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BecomeAWizzard.Api.Controllers;

[Authorize, ApiController, Route("api/game")]
public class GameController(GameService gameService) : ControllerBase
{
    [HttpPost("attempts")]
    public async Task<ActionResult> Start(StartAttemptRequest request)
    {
        try { return Ok(await gameService.StartAsync(User.GetUserId(), request.QuizId)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("attempts/{id:int}")]
    public async Task<ActionResult> State(int id) => await gameService.GetStateAsync(id, User.GetUserId()) is { } state ? Ok(state) : NotFound();

    [HttpPost("attempts/{id:int}/answers")]
    public async Task<ActionResult> Answer(int id, SubmitAnswerRequest request)
    {
        try { return Ok(await gameService.SubmitAnswerAsync(id, User.GetUserId(), request)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException) { return BadRequest(new { message = ex.Message }); }
    }
}

