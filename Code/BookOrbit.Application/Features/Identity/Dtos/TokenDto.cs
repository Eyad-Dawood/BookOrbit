namespace BookOrbit.Application.Features.Identity.Dtos;

public class TokenDto
{
    public string? AccessToken { get; set; } = null;
    public string? RefreshToken { get; set; } = null;
    public DateTime ExpiresOnUtc { get; set; } = default;
}