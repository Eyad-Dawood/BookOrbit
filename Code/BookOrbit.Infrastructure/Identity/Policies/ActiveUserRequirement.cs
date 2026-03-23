namespace BookOrbit.Infrastructure.Identity.Policies;

public class ActiveUserRequirement : IAuthorizationRequirement;

public class ActiveUserHandler(
    ILogger<ActiveUserHandler> logger,
    UserManager<AppUser> userManager) : AuthorizationHandler<ActiveUserRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            logger.LogWarning("Authorization failed: userId not found in token");
            return;
        }

        var userConfirmedEmail = await userManager.Users
            .FirstOrDefaultAsync(u=>u.Id==userId&&u.EmailConfirmed);

        if(userConfirmedEmail is null)
        {
            context.Fail();
            logger.LogWarning("Authorization failed: userId : [{userId}] not found in system or hasnt confirm his email",userId);
            return;
        }

        context.Succeed(requirement);
        return;
    }
}