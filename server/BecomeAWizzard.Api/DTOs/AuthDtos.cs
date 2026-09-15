using System.ComponentModel.DataAnnotations;

namespace BecomeAWizzard.Api.DTOs;

public record RegisterRequest(
    [Required, MaxLength(60)] string DisplayName,
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password);

public record LoginRequest([Required, EmailAddress] string Email, [Required] string Password);
public record UserDto(int Id, string DisplayName, string Email, int TotalXp);

