namespace BookOrbit.Domain.Common.Constants;

static public class PhoneNumberValidationConstants
{
    public const int MinLength = 11;
    public const int MaxLength = 12;
    static public readonly List<string> Prefixes = new() { "01", "201" };

}

