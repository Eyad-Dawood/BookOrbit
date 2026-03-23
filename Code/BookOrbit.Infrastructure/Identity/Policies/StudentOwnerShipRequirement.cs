namespace BookOrbit.Infrastructure.Identity.Policies;
public class StudentOwnerShipRequirement : IAuthorizationRequirement;

public class StudentOwnerShipHandler(
    ILogger<StudentOwnerShipHandler> logger,
    IAppDbContext dbContext,
    IHttpContextAccessor contextAccessor) : AuthorizationHandler<StudentOwnerShipRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        StudentOwnerShipRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogWarning("Authorization failed: userId not found in token");
            context.Fail();
            return;
        }

        //Admin Bybass
        if (context.User.IsInRole(IdentityRoles.admin.ToString()))
        {
            context.Succeed(requirement);
            return;
        }

        if (!context.User.IsInRole(IdentityRoles.student.ToString()))
        {
            logger.LogWarning("Authorization failed: user is not Student/Admin");
            context.Fail();
            return;
        }

        var idRouteValue = contextAccessor.HttpContext?
            .Request.RouteValues["id"]?.ToString();

        if (!Guid.TryParse(idRouteValue, out var routeStudentId))
        {
            logger.LogWarning("Authorization failed: invalid or missing route id");
            context.Fail();
            return;
        }
        

        var isOwner = await dbContext.Students
            .AnyAsync(s => s.Id == routeStudentId && s.UserId == userId);

        if (!isOwner)
        {
            logger.LogWarning("Authorization failed: user is not owner of this resource");
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}