namespace BookOrbit.Domain.Books.ValueObjects;
public record BookTitle(string Value) : ValueObject<string>(Value)
{

    private static readonly Regex BookTitleRegex =
       new(@"^[A-Za-z\u0600-\u06FF ]{3,200}$", RegexOptions.Compiled);

    public static string Normalize(string value)
    {
        return value
          .Trim();
    }
    private static Result<string> Validate(string value)
    {
        if (!BookTitleRegex.IsMatch(value))
            return BookErrors.InvalidTitle;

        return value;
    }
    public static Result<BookTitle> Create(string? bookTitle)
    {
        // Hot path, so we check for null or whitespace before normalization and validation to avoid unnecessary processing.
        if (string.IsNullOrWhiteSpace(bookTitle))
            return BookErrors.TitleRequired;


        var normalized = Normalize(bookTitle);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new BookTitle(validationResult.Value);

        return validationResult.Errors;
    }

}