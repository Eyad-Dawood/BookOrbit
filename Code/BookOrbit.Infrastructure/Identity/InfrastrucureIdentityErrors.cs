
namespace BookOrbit.Infrastructure.Identity;
static public class InfrastrucureIdentityErrors
{
    private const string ClassName = "User";

    static public readonly Error UserNotFoundByEmail = ApplicationCommonErrors.NotFoundClass(ClassName, "Email", "Email");
    static public readonly Error UserNotFoundById = ApplicationCommonErrors.NotFoundClass(ClassName, "Id", "Id");
    static public readonly Error EmailNotConfirmed = ApplicationCommonErrors.CustomConflict(ClassName, "UserEmailNotConfirmed", "This user hasnt confirm his email yet.");
    static public readonly Error InvalidLoginAttempt = ApplicationCommonErrors.CustomConflict(ClassName, "FaildLoginAttempt", "Email or Password are incorrect.");
}