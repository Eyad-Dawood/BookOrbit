namespace BookOrbit.Domain.Students.ValueObjects;

public record StudentName : ValueObject<string>
{
    //Arabic , english , spaces
    private static readonly Regex NameRegex =
   new(@"^[A-Za-z ]+$", RegexOptions.Compiled);

    private StudentName(string Value) : base(Value) { }

    public static string Normalize(string value)
    {
        return value
            .Trim();
    }

    private static Result<string> Validate(string value)
    {
        if (value.Length > StudentValidationConstants.NameMaxLength || value.Length < StudentValidationConstants.NameMinLength)
            return StudentErrors.InvalidName;

        if (!NameRegex.IsMatch(value))
            return StudentErrors.InvalidName;

        return value;
    }
    public static Result<StudentName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return StudentErrors.NameRequired;

        var normalized = Normalize(name);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new StudentName(validationResult.Value);

        return validationResult.Errors;

    }

    public static implicit operator string(StudentName? user)
    {
        if (user is null) return string.Empty;

        return user.Value;
    }
}

