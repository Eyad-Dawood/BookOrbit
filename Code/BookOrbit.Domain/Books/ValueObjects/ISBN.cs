namespace BookOrbit.Domain.Books.ValueObjects;

public record ISBN : ValueObject<string>
{
    private static readonly Regex IsbnRegex =
        new(@"^(?:\d{9}[\dXx]|\d{13})$", RegexOptions.Compiled);

    private ISBN(string value):base(value){}


    private static string Normalize(string value)
    {
        return value
            .Trim()
            .Replace("-", "")
            .Replace(" ", "");
    }
    private static Result<string> Validate(string value)
    {
        if (!IsbnRegex.IsMatch(value))
            return BookErrors.InvalidISBN;

        return value;
    }
    public static Result<ISBN> Create(string? isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return BookErrors.ISBNRequired;

        var normalized = Normalize(isbn);
        var validationResult = Validate(normalized);


        if (validationResult.IsSuccess)
            return new ISBN(validationResult.Value);

        return validationResult.Errors;
    }

    public static implicit operator string(ISBN? user)
    {
        if (user is null) return string.Empty;

        return user.Value;
    }
}