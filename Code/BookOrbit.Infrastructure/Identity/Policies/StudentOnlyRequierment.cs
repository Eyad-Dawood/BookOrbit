namespace BookOrbit.Infrastructure.Identity.Policies;
public class StudentOnlyRequierment : IAuthorizationRequirement;

public class StudentOnlyHandler(
    ILogger<StudentOnlyHandler> logger,
    ICurrentUser currentUser) : AuthorizationHandler<StudentOnlyRequierment>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StudentOnlyRequierment requirement)
    {
        var userId = currentUser.Id;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogWarning("Authorization failed: userId not found in token");
            context.Fail();
            return Task.CompletedTask;
        }

        //NO ADMIN Bybass

        if (!currentUser.IsInRole(IdentityRoles.student.ToString()))
        {
            logger.LogWarning("Authorization failed: user is not Student");
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
