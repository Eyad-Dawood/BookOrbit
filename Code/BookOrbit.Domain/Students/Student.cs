namespace BookOrbit.Domain.Students;

public class Student : AuditableEntity
{
    public string Name { get; }
    public PhoneNumber? PhoneNumber { get; }
    public TelegramUserId? TelegramUserId { get; }
    public UniversityMail UniversityMail { get; }
    public Url PersonalPhotoUrl { get; }
    public int Points { get; private set; }
    public DateOnly JoinDate { get; }
    public StudentState State { get; private set; }

#pragma warning disable CS8618
    private Student()
    { }


    private Student(
        Guid id,
        string name,
        UniversityMail universityMail,
        Url personalPhotoUrl,
        DateOnly joinDate,
        PhoneNumber? phoneNumber = null,
        TelegramUserId? telegramUsername = null) : base(id)
    {
        Name = name;
        UniversityMail = universityMail;
        PersonalPhotoUrl = personalPhotoUrl;
        JoinDate = joinDate;
        PhoneNumber = phoneNumber;
        TelegramUserId = telegramUsername;
        State = StudentState.Pending;
        Points = 0;
    }


    public static Result<Student> Create(
        Guid id,
        string name,
        string universityMailAddress,
        string personalPhotoUrlAddress,
        DateOnly joinDate,
        DateTime currentTime, // For Testing purposes, to avoid using DateTime.UtcNow directly
        string? phoneNumber = null,
        string? telegramUsername = null)
    {
        if (id == Guid.Empty)
            return StudentErrors.IdRequired;

        if(string.IsNullOrWhiteSpace(name))
            return StudentErrors.NameRequired;

        name = name.Trim();
        if (name.Length > StudentValidationConstants.NameMaxLength || name.Length < StudentValidationConstants.NameMinLength)
            return StudentErrors.InvalidName;

        if (joinDate > DateOnly.FromDateTime(currentTime))
            return StudentErrors.InvalidJoinDate;

        if(string.IsNullOrWhiteSpace(phoneNumber)&&string.IsNullOrWhiteSpace(telegramUsername))
            return StudentErrors.AtLeastOneCommunicationMethod;


        var universityMailResult = UniversityMail.Create(universityMailAddress);
        if(universityMailResult.IsFailure)
            return universityMailResult.Errors;
        
        var personalPhotoUrlResult = Url.Create(personalPhotoUrlAddress);
        if(personalPhotoUrlResult.IsFailure)
            return personalPhotoUrlResult.Errors;


        PhoneNumber? OphoneNumber = null;
        TelegramUserId? OtelegramUsername = null;

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            var phoneNumberResult = PhoneNumber.Create(phoneNumber);
            if (phoneNumberResult.IsFailure)
                return phoneNumberResult.Errors;
            else
                OphoneNumber = phoneNumberResult.Value;
        }

        if (!string.IsNullOrWhiteSpace(telegramUsername))
        {
            var telegramUserIdResult = TelegramUserId.Create(telegramUsername);
            if (telegramUserIdResult.IsFailure)
                return telegramUserIdResult.Errors;
            else
                OtelegramUsername = telegramUserIdResult.Value;
        }


        return new Student(
            id,
            name,
            universityMailResult.Value,
            personalPhotoUrlResult.Value,
            joinDate,
            OphoneNumber,
            OtelegramUsername);
    }


}

