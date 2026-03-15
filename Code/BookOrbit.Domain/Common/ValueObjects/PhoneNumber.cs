namespace BookOrbit.Domain.Common.ValueObjects;

public record PhoneNumber
{
    public string Value { get; }
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string phoneNumber)
    {

        if (string.IsNullOrWhiteSpace(phoneNumber))
            return PhoneNumberErrors.RequiredPhoneNumber;

        phoneNumber = phoneNumber.Replace(" ", "").Replace("-", "");

        const string pattern = @"^(?:01|201)[0125][0-9]{8}$";

        if (!Regex.IsMatch(phoneNumber, pattern))
            return PhoneNumberErrors.InvalidPhoneNumber;

        return new PhoneNumber(phoneNumber);
    }
}

static public class PhoneNumberErrors {

    private const string className = nameof(PhoneNumber);

    public static readonly Error InvalidPhoneNumber = CommonErrors.InvalidProp(
                className,
                "Value",
                "Phone number",
                "It must be a valid Egyptian phone number."
            );

    public static readonly Error RequiredPhoneNumber = CommonErrors.RequiredProp(
                className,
                "Value",
                "Phone number"
            );
}