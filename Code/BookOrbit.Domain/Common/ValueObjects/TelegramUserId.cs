namespace BookOrbit.Domain.Common.ValueObjects;

public record TelegramUserId
{
    public string Value { get; }

    private TelegramUserId(string value)
    {
        Value = value;
    }

    public static Result<TelegramUserId> Create(string telegramUserId)
    {
        if (string.IsNullOrWhiteSpace(telegramUserId))
            return TelegramUserIdErrors.TelegramUserIdRequired;


        const string pattern = @"^(?=(?:[0-9_]*[a-z]){3})[a-z0-9_]{5,}$";

        if (!Regex.IsMatch(telegramUserId, pattern))
            return TelegramUserIdErrors.InvalidTelegramUserId;

        return new TelegramUserId(telegramUserId);
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

