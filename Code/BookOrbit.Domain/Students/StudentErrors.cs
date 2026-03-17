namespace BookOrbit.Domain.Students;

static public class StudentErrors
{
    private const string ClassName = nameof(Student);

    #region General
    static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName,"Id","Id");
    static public readonly Error NameRequired = CommonErrors.RequiredProp(ClassName,"Name","Name");
    static public readonly Error InvalidName = CommonErrors.InvalidProp(ClassName, "Name", "Name", $"It must be between {StudentValidationConstants.NameMinLength} and {StudentValidationConstants.NameMaxLength} characters");
    static public readonly Error InvalidJoinDate = CommonErrors.InvalidProp(ClassName, "JoinDate", "Join Date", "It cannot be before creation date request");
    static public readonly Error InvalidState = CommonErrors.InvalidProp(ClassName, "StudentState", "Student State", $"Invalid state value");
    static public readonly Error AtLeastOneCommunicationMethod = CommonErrors.Custom(ClassName, "AtLeastOneCommunicationMethod", "At least one communication method (Phone Number or Telegram Username) must be provided.");
    static public readonly Error StudentEmailExists = CommonErrors.Custom(ClassName, "StudentExists", "A student with the same university mail already exists.");
    static public readonly Error StudentTelegramIdExists = CommonErrors.Custom(ClassName, "StudentExists", "A student with the same telegram user Id already exists.");
    static public readonly Error StudentPhoneNumberExists = CommonErrors.Custom(ClassName, "StudentExists", "A student with the same phone number user Id already exists.");
    static public readonly Error PersonalImageRequired = CommonErrors.RequiredProp(ClassName,"PersonalImageId","Personal Image");
    static public readonly Error PersonalImageNotFound = CommonErrors.Custom(ClassName, "PersonalImageNotFound", "The specified personal image was not found.");
    #endregion

    #region Mail
    static public readonly Error UniversityMailRequired = CommonErrors.RequiredProp(ClassName,"UniversityMail","University Mail");
    static public readonly Error InvalidUniversityMail = CommonErrors.InvalidProp(ClassName, "UniversityMail", "University Mail", "It must be a valid email address , and end with @std.mans.edu.eg");
    #endregion

    #region Logic

    public static Error InvalidStateTransition(StudentState currentState, StudentState newState)=>
        CommonErrors.InvalidStateTransition(ClassName,currentState.ToString(),newState.ToString());
    
    #endregion
}

