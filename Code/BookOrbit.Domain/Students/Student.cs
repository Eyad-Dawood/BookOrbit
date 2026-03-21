namespace BookOrbit.Domain.Students;

public class Student : AuditableEntity
{
    public StudentName Name { get; private set; }
    public PhoneNumber? PhoneNumber { get; }
    public TelegramUserId? TelegramUserId { get; }
    public UniversityMail UniversityMail { get; }
    public string PersonalPhotoFileName { get; }
    public int Points { get; private set; }
    public DateTimeOffset? JoinDateUtc { get; private set; } = null;
    public StudentState State { get; private set; }

   //Identity
   public string UserId { get; }

#pragma warning disable CS8618
    private Student()
    { }


    private Student(
        Guid id,
        StudentName name,
        UniversityMail universityMail,
        string personalPhotoFileName,
        string userId,
        PhoneNumber? phoneNumber = null,
        TelegramUserId? telegramUserId = null) : base(id)
    {
        Name = name;
        UniversityMail = universityMail;
        PersonalPhotoFileName = personalPhotoFileName;
        UserId = userId;
        PhoneNumber = phoneNumber;
        TelegramUserId = telegramUserId;
        State = StudentState.UnVerified;
        Points = 0;
    }


    public static Result<Student> Create(
        Guid id,
        StudentName name,
        UniversityMail universityMail,
        string personalPhotoFileName,
        string userId,
        PhoneNumber? phoneNumber = null,
        TelegramUserId? telegramUserId = null)
    {
        if (id == Guid.Empty)
            return StudentErrors.IdRequired;

        if (name is null)
            return StudentErrors.NameRequired;

        if (universityMail is null)
            return StudentErrors.UniversityMailRequired;

        if (phoneNumber is null && telegramUserId is null)
            return StudentErrors.AtLeastOneCommunicationMethod;

        if(string.IsNullOrWhiteSpace(userId))
            return StudentErrors.UserIdRequired;

        if (string.IsNullOrWhiteSpace(personalPhotoFileName))
            return StudentErrors.PersonalImageRequired;

        return new Student(
            id,
            name,
            universityMail,
            personalPhotoFileName,
            userId,
            phoneNumber,
            telegramUserId);
    }


    public Result<Updated> Update(StudentName name)
    {
        //No need to return a result because Value object already validated
        //BUTTTTTTT it will return a result to keep the pattern consistant
        Name = name;

        return Result.Updated;
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
        if (joinDateUtc < CreatedAtUtc)
            return StudentErrors.InvalidJoinDate;


        var result = UpdateState(StudentState.Approved);

        if (result.IsFailure)
            return result;

        JoinDateUtc = joinDateUtc;
        return result;
    }

    public Result<Updated> Activate() =>
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