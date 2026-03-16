namespace BookOrbit.Domain.Common.ValueObjects;

public record TelegramUserId : ValueObject<string>
{
    private static readonly Regex TelegramUserIdRegex =
       new(@"^(?=(?:[0-9_]*[a-z]){3})[a-z0-9_]{5,}$", RegexOptions.Compiled);

    private TelegramUserId(string value) : base(value){}
    private static string Normalize(string value)
    {
        return 
            value.Trim()
            .ToLower();
    }
    private static Result<string> Validate(string value)
    {
        if (!TelegramUserIdRegex.IsMatch(value))
            return TelegramUserIdErrors.InvalidTelegramUserId;

        return value;
    }
    public static Result<TelegramUserId> Create(string telegramUserId)
    {
        if (string.IsNullOrWhiteSpace(telegramUserId))
            return TelegramUserIdErrors.TelegramUserIdRequired;

        var normalized = Normalize(telegramUserId);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new TelegramUserId(validationResult.Value);

        return validationResult.Errors;
    }
}

public static class TelegramUserIdErrors
{
    private const string className = nameof(TelegramUserId);

    public static readonly Error InvalidTelegramUserId = CommonErrors.InvalidProp(
                className,
                "Value",
                "Telegram User ID",
                "It must be a valid telegram user Id.");

    public static readonly Error TelegramUserIdRequired = CommonErrors.RequiredProp(
                className,
                "Value",
                "Telegram User ID");

}

