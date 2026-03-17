namespace BookOrbit.Domain.Students;

public class Student : AuditableEntity
{
    public string Name { get; }
    public PhoneNumber? PhoneNumber { get; }
    public TelegramUserId? TelegramUserId { get; }
    public UniversityMail UniversityMail { get; }
    public Url PersonalPhotoUrl { get; }
    public int Points { get; private set; }
    public DateTimeOffset JoinDateUtc { get; private set; }
    public StudentState State { get; private set; }

#pragma warning disable CS8618
    private Student()
    { }


    private Student(
        Guid id,
        string name,
        UniversityMail universityMail,
        Url personalPhotoUrl,
        PhoneNumber? phoneNumber = null,
        TelegramUserId? telegramUserId = null) : base(id)
    {
        Name = name;
        UniversityMail = universityMail;
        PersonalPhotoUrl = personalPhotoUrl;
        PhoneNumber = phoneNumber;
        TelegramUserId = telegramUserId;
        State = StudentState.UnVerified;
        Points = 0;
    }


    public static Result<Student> Create(
        Guid id,
        string name,
        string universityMailAddress,
        string personalPhotoUrlAddress,
        string? phoneNumber = null,
        string? telegramUserId = null)
    {
        if (id == Guid.Empty)
            return StudentErrors.IdRequired;

        if (string.IsNullOrWhiteSpace(name))
            return StudentErrors.NameRequired;

        name = name.Trim();
        if (name.Length > StudentValidationConstants.NameMaxLength || name.Length < StudentValidationConstants.NameMinLength)
            return StudentErrors.InvalidName;

        if (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(telegramUserId))
            return StudentErrors.AtLeastOneCommunicationMethod;

        var universityMailResult = UniversityMail.Create(universityMailAddress);
        if (universityMailResult.IsFailure)
            return universityMailResult.Errors;

        var personalPhotoUrlResult = Url.Create(personalPhotoUrlAddress);
        if (personalPhotoUrlResult.IsFailure)
            return personalPhotoUrlResult.Errors;


        PhoneNumber? parsedPhoneNumber = null;
        TelegramUserId? parsedTelegramUserId = null;

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            var phoneNumberResult = PhoneNumber.Create(phoneNumber);
            if (phoneNumberResult.IsFailure)
                return phoneNumberResult.Errors;
            else
                parsedPhoneNumber = phoneNumberResult.Value;
        }

        if (!string.IsNullOrWhiteSpace(telegramUserId))
        {
            var telegramUserIdResult = TelegramUserId.Create(telegramUserId);
            if (telegramUserIdResult.IsFailure)
                return telegramUserIdResult.Errors;
            else
                parsedTelegramUserId = telegramUserIdResult.Value;
        }


        return new Student(
            id,
            name,
            universityMailResult.Value,
            personalPhotoUrlResult.Value,
            parsedPhoneNumber,
            parsedTelegramUserId);
    }


    private bool CanTransitionToState(StudentState newState)
    {
        return State switch
        {
            StudentState.UnVerified => newState is StudentState.Pending,
            StudentState.Pending => newState is StudentState.Approved or StudentState.Rejected,
            StudentState.Approved => newState is StudentState.Active or StudentState.Banned or StudentState.Suspended,
            StudentState.Active => newState is StudentState.Banned or StudentState.Suspended,
            StudentState.Rejected => false,
            StudentState.Banned => false,
            StudentState.Suspended => newState is StudentState.Active,
            _ => false
        };
    }
    private Result<Updated> UpdateState(StudentState newState)
    {
        if (!CanTransitionToState(newState))
                return StudentErrors.InvalidStateTransition(State, newState);

        State = newState;

        return Result.Updated;
    }

    public Result<Updated> Approve(DateTimeOffset joinDateUtc)
    {
        if(joinDateUtc<CreatedAtUtc)
            return StudentErrors.InvalidJoinDate;


        var result = UpdateState(StudentState.Approved);
        
        if(result.IsFailure)
            return result;

        JoinDateUtc = joinDateUtc;
        return result;
    }

    public Result<Updated> Activate()=>
        UpdateState(StudentState.Active);

    public Result<Updated> Reject() =>
        UpdateState(StudentState.Rejected);

    public Result<Updated> Ban() =>
        UpdateState(StudentState.Banned);

    public Result<Updated> Suspend() =>
        UpdateState(StudentState.Suspended);

    public Result<Updated> Verify() =>
        UpdateState(StudentState.Pending);
}