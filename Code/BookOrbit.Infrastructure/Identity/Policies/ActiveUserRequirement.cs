
namespace BookOrbit.Infrastructure.Identity.Policies;

public class ActiveUserRequirement : IAuthorizationRequirement;

public class ActiveUserHandler(
    IIdentityService identityService,
    ILogger<ActiveUserHandler> logger) : AuthorizationHandler<ActiveUserRequirement>
{
    protected async override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            logger.LogWarning("Authorization failed: userId not found in token");
            return;
        }

        var user = await identityService.GetUserByIdAsync(userId);

        if (user.IsFailure)
        {
            context.Fail();
            logger.LogWarning("Authorization failed: user {UserId} was not found in the system", userId);
            return;
        }

        context.Succeed(requirement);
    }
}