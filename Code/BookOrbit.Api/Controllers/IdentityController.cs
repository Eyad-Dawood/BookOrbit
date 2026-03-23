

namespace BookOrbit.Api.Controllers;

[Route("api/v{version:apiVersion}/identity")]
[ApiVersion("1.0")]
[ApiController]
public class IdentityController(ISender sender, IEmailService emailService) : ApiController
{
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Generates an access and refresh token for a valid user.")]
    [EndpointDescription("Authenticates a user using provided credentials and returns a JWT token pair.")]
    [EndpointName("GenerateToken")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.SensitiveRateLimmitingPolicyName)]
    public async Task<ActionResult<TokenDto>> GenerateToken([FromBody] GenerateTokenQuery request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        return result.Match(
            Ok,
            e => Problem(e, HttpContext));
    }

    [HttpPost("token/refresh")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Refreshes access token using a valid refresh token.")]
    [EndpointDescription("Exchanges an expired access token and a valid refresh token for a new token pair.")]
    [EndpointName("RefreshToken")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.SensitiveRateLimmitingPolicyName)]
    public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] RefreshTokenQuery request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        return result.Match(
            Ok,
            e => Problem(e, HttpContext));
    }


    [HttpGet("users/me")]
    [Authorize(Policy = PoliciesNames.RegisteredUserPolicy)]
    [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    [MapToApiVersion("1.0")]
    [EndpointSummary("Gets the current authenticated user's info.")]
    [EndpointDescription("Returns user information for the currently authenticated user based on the access token.")]
    [EndpointName("GetCurrentUser")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<AppUserDto>> GetCurrentUser(CancellationToken ct)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await sender.Send(
            new GetUserByIdQuery(userId),
            ct);

        return result.Match(
            Ok,
            e => Problem(e, HttpContext));
    }



    [HttpPost("users/{id}/send-email-confirmation")]
    [Authorize(Policy = PoliciesNames.RegisteredUserOwnershipPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    [MapToApiVersion("1.0")]
    [EndpointSummary("Sends email confirmation link to the user.")]
    [EndpointDescription("Generates a confirmation token and sends an email with a confirmation link.")]
    [EndpointName("SendEmailConfirmation")]
    [EnableRateLimiting(ApiConstants.SensitiveRateLimmitingPolicyName)]
    public async Task<ActionResult> SendEmailConfirmation([FromRoute] string Id, CancellationToken ct)
    {
        var confirmationResult = await sender.Send(new GenerateEmailConfirmationTokenCommand(Id), ct);

        if (confirmationResult.IsFailure)
            return Problem(confirmationResult.Errors, HttpContext);

        var link = Url.RouteUrl("ConfirmEmail",
        new { Id, token = confirmationResult.Value.encodedConfirmationToken, version = "1.0" },
        Request.Scheme);

        await emailService.SendEmailAsync(
            confirmationResult.Value.email,
            "Confirm your email",
            $"Click here: <a href='{link}'>Confirm</a>");

        return Ok("Email Sent");
    }


    [HttpGet("confirm-email", Name = "ConfirmEmail")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    [MapToApiVersion("1.0")]
    [EndpointSummary("Confirms user email.")]
    [EndpointDescription("Validates the email confirmation token and confirms the user's email.")]
    [EndpointName("ConfirmEmail")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult> ConfirmEmail(
    [FromQuery] string Id,
    [FromQuery] string token,
    CancellationToken ct)
    {
        var result = await sender.Send(new ConfirmEmailCommand(Id, token), ct);

        if (result.IsFailure)
            return Problem(result.Errors, HttpContext);

        return Ok("Email confirmed successfully");
    }



}