namespace BookOrbit.Domain.Students;

static public class StudentErrors
{
    private const string ClassName = nameof(Student);

    #region General
    static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName,"Id","Id");
    static public readonly Error NameRequired = CommonErrors.RequiredProp(ClassName,"Name","Name");
    static public readonly Error InvalidName = CommonErrors.InvalidProp(ClassName, "Name", "Name", $"It must be between {StudentValidationConstants.NameMinLength} and {StudentValidationConstants.NameMaxLength} characters");
    static public readonly Error InvalidJoinDate = CommonErrors.InvalidProp(ClassName, "JoinDate", "Join Date", "It cannot be before creation date request");
    static public readonly Error InvalidState = CommonErrors.InvalidProp(ClassName, "State", "State", $"It must be one of the following: {string.Join(", ", Enum.GetNames(typeof(StudentState)))}");
    static public readonly Error AtLeastOneCommunicationMethod = CommonErrors.Custom(ClassName, "AtLeastOneCommunicationMethod", "At least one communication method (Phone Number or Telegram Username) must be provided.");
    #endregion

    #region Mail
    static public readonly Error UniversityMailRequired = CommonErrors.RequiredProp(ClassName,"UniversityMail","University Mail");
    static public readonly Error InvalidUniversityMail = CommonErrors.InvalidProp(ClassName, "UniversityMail", "University Mail", "It must end with @std.mans.edu.eg");
    #endregion

    #region Logic

    public static Error InvalidStateTransition(StudentState currentState, StudentState newState)=>
        CommonErrors.InvalidStateTransition(ClassName,currentState.ToString(),newState.ToString());
    
    #endregion
}

