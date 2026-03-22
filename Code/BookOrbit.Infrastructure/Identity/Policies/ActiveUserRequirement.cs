namespace BookOrbit.Infrastructure.Identity.Policies;

public class ActiveUserRequirement : IAuthorizationRequirement;

public class ActiveUserHandler(
    ILogger<ActiveUserHandler> logger) : AuthorizationHandler<ActiveUserRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            logger.LogWarning("Authorization failed: userId not found in token");
            return Task.CompletedTask;
        }

        //No need right now Its enough to have id in token
        //Maybe when i add User state in database
        //var user = await identityService.GetUserByIdAsync(userId);

        //if (user.IsFailure)
        //{
        //    context.Fail();
        //    logger.LogWarning("Authorization failed: user {UserId} was not found in the system", userId);
        //    return;
        //}

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}