namespace BookOrbit.Api.Controllers.Students;

[Route("api/v{version:apiVersion}/students")]
[ApiVersion("1.0")]
[Authorize]
public class StudentManagementController(
    ISender sender) : ApiController
{
    [HttpPatch("{id:guid}/approve")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Approve a student.")]
    [EndpointDescription("Approves a student account after verifying that the email is confirmed.")]
    [EndpointName("ApproveStudent")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult> ApproveStudent([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await sender.Send(
            new ApproveStudentCommand(
                id),
            ct);

        return result.Match(
            response => NoContent(),
            e => Problem(e, HttpContext));
    }



    [HttpPatch("{id:guid}/activate")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Activate a student.")]
    [EndpointDescription("Activate a student account after being approved.")]
    [EndpointName("ActivateStudent")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult> ActivateStudent([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await sender.Send(
            new ActivateStudentCommand(
                id),
            ct);

        return result.Match(
            response => NoContent(),
            e => Problem(e, HttpContext));
    }


    [HttpPatch("{id:guid}/ban")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Ban a student.")]
    [EndpointDescription("Ban a student account .")]
    [EndpointName("BanStudent")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult> BanStudent([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await sender.Send(
            new BanStudentCommand(
                id),
            ct);

        return result.Match(
            response => NoContent(),
            e => Problem(e, HttpContext));
    }


    [HttpPatch("{id:guid}/reject")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Reject a student.")]
    [EndpointDescription("Reject a student account .")]
    [EndpointName("RejectStudent")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult> RejectStudent([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await sender.Send(
            new RejectStudentCommand(
                id),
            ct);

        return result.Match(
            response => NoContent(),
            e => Problem(e, HttpContext));
    }


    [HttpPatch("{id:guid}/unban")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Un ban a student.")]
    [EndpointDescription("Un ban a student account .")]
    [EndpointName("UnBanStudent")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult> UnBanStudent([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await sender.Send(
            new UnBanStudentCommand(
                id),
            ct);

        return result.Match(
            response => NoContent(),
            e => Problem(e, HttpContext));
    }

}