namespace BookOrbit.Domain.Students.ValueObjects;

public record UniversityMail
{
    public string Value { get; }
    private UniversityMail(string value)
    {
        Value = value;
    }
    public static Result<UniversityMail> Create(string email)
    {
        email = email.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(email))
            return StudentErrors.UniversityMailRequired;

        if (!email.EndsWith("@std.mans.edu.eg"))
            return StudentErrors.InvalidUniversityMail;

        return new UniversityMail(email);
    }

}

