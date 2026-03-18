namespace BookOrbit.Domain.Identity;

public sealed class RefreshToken : AuditableEntity
{
    public string? Token { get; }
    public string? UserId { get; }
    public DateTimeOffset ExpiresOnUtc { get; }
    public bool IsRevoked { get; private set; }
    public bool IsUsed { get; private set; }

    private RefreshToken()
    { }

    private RefreshToken(
        Guid id,
        string? token,
        string? userId,
        DateTimeOffset expiresOnUtc)
        : base(id)
    {
        Token = token;
        UserId = userId;
        ExpiresOnUtc = expiresOnUtc;
        IsRevoked = false;
        IsUsed = false;
    }

    public static Result<RefreshToken> Create(
        Guid id, 
        string? token, 
        string? userId, 
        DateTimeOffset expiresOnUtc)
    {
        if (id == Guid.Empty)
        {
            return RefreshTokenErrors.IdRequired;
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            return RefreshTokenErrors.TokenRequired;
        }

        if (string.IsNullOrWhiteSpace(userId))
        {
            return RefreshTokenErrors.UserIdRequired;
        }

        if (expiresOnUtc <= DateTimeOffset.UtcNow)
        {
            return RefreshTokenErrors.InvalidExpirationDate;
        }

        return new RefreshToken(
            id,
            token,
            userId,
            expiresOnUtc);
    }

    public Result<Updated> MarkAsRevoke()
    {
        if (IsRevoked)
            return RefreshTokenErrors.AlreadRevoked;

        IsRevoked = true;

        return Result.Updated;
    }

    public Result<Updated> MarkAsUsed()
    {
        if (IsRevoked)
            return RefreshTokenErrors.CannotUseRevokedToken;

        if (IsUsed)
            return RefreshTokenErrors.AlreadyUsed;

        IsUsed = true;

        return Result.Updated;
    }
}