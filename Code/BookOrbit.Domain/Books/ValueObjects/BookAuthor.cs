namespace BookOrbit.Domain.Books.ValueObjects;
public record BookAuthor(string Value) : ValueObject<string>(Value)
{

    private static readonly Regex BookAuthorRegex =
       new(@"^[A-Za-z\u0600-\u06FF ]{3,150}$", RegexOptions.Compiled);

    public static string Normalize(string value)
    {
        return value
          .Trim();
    }
    private static Result<string> Validate(string value)
    {
        if (!BookAuthorRegex.IsMatch(value))
            return BookErrors.InvalidAuthor;

        return value;
    }
    public static Result<BookAuthor> Create(string? bookAuthor)
    {
        // Hot path, so we check for null or whitespace before normalization and validation to avoid unnecessary processing.
        if (string.IsNullOrWhiteSpace(bookAuthor))
            return BookErrors.AuthorRequired;


        var normalized = Normalize(bookAuthor);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new BookAuthor(validationResult.Value);

        return validationResult.Errors;
    }

}