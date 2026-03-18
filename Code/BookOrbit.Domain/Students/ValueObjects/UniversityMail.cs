namespace BookOrbit.Domain.Students.ValueObjects;

public record UniversityMail : ValueObject<string>
{
    private static readonly Regex UniversityMailRegex =
   new(@"^[A-Za-z0-9._%+-]+@std\.mans\.edu\.eg$", RegexOptions.Compiled);

    private UniversityMail(string value) : base(value) { }
    public static string Normalize(string value)
    {
        return value
            .Trim()
            .ToLower();
    }
    private static Result<string> Validate(string value)
    {
        if (!UniversityMailRegex.IsMatch(value))
            return StudentErrors.InvalidUniversityMail;

        return value;
    }
    public static Result<UniversityMail> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return StudentErrors.UniversityMailRequired;

        var normalized = Normalize(email);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new UniversityMail(validationResult.Value);

        return validationResult.Errors;
    }

    public static implicit operator string(UniversityMail? user)
    {
        if (user is null) return string.Empty;

        return user.Value;
    }
}

