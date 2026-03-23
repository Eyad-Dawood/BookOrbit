namespace BookOrbit.Infrastructure.Identity.Policies;

public class RegisteredUserRequirement : IAuthorizationRequirement;

public class RegisteredUserHandler(ILogger<AdminOnlyHandler> logger) : AuthorizationHandler<RegisteredUserRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RegisteredUserRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            logger.LogWarning("Authorization failed: userId not found in token");
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}