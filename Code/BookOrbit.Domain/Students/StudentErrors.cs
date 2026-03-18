namespace BookOrbit.Domain.Students;

static public class StudentErrors
{
    private const string ClassName = nameof(Student);

    #region General
    static public readonly Error IdRequired = DomainCommonErrors.RequiredProp(ClassName,"Id","Id");
    static public readonly Error NameRequired = DomainCommonErrors.RequiredProp(ClassName,"Name","Name");
    static public readonly Error InvalidName = DomainCommonErrors.InvalidProp(ClassName, "Name", "Name", $"It must be between {StudentValidationConstants.NameMinLength} and {StudentValidationConstants.NameMaxLength} characters");
    static public readonly Error InvalidJoinDate = DomainCommonErrors.InvalidProp(ClassName, "JoinDate", "Join Date", "It cannot be before creation date request");
    static public readonly Error InvalidState = DomainCommonErrors.InvalidProp(ClassName, "StudentState", "Student State", $"Invalid state value");
    static public readonly Error AtLeastOneCommunicationMethod = DomainCommonErrors.Custom(ClassName, "AtLeastOneCommunicationMethod", "At least one communication method (Phone Number or Telegram Username) must be provided.");
    static public readonly Error PersonalImageRequired = DomainCommonErrors.RequiredProp(ClassName,"PersonalImageId","Personal Image");
    #endregion

    #region Mail
    static public readonly Error UniversityMailRequired = DomainCommonErrors.RequiredProp(ClassName,"UniversityMail","University Mail");
    static public readonly Error InvalidUniversityMail = DomainCommonErrors.InvalidProp(ClassName, "UniversityMail", "University Mail", "It must be a valid email address , and end with @std.mans.edu.eg");
    #endregion

    #region Logic

    public static Error InvalidStateTransition(StudentState currentState, StudentState newState)=>
        DomainCommonErrors.InvalidStateTransition(ClassName,currentState.ToString(),newState.ToString());
    
    #endregion
}

