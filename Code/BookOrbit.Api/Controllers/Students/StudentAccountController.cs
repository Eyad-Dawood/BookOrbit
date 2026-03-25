namespace BookOrbit.Api.Controllers.Students;

[Route("api/v{version:apiVersion}/students")]
[ApiVersion("1.0")]
[Authorize]
public class StudentAccountController(
    ISender sender,
    ICurrentUser currentUser,
    ImageHelper imageHelper) : ApiController
{
    [HttpGet("me")]
    [Authorize(Policy = PoliciesNames.StudentOnlyPolicy)]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves the current student.")]
    [EndpointDescription("Returns detailed information about the current logged in student if found.")]
    [EndpointName("GetCurrentStudent")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<StudentDto>> GetCurrentStudent(CancellationToken ct)
    {
        var userId = currentUser.Id;

        var query = new GetStudentByUserIdQuery(userId);

        var result = await sender.Send(query, ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }


    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Register a new student with an associated account.")]
    [EndpointDescription("Registers a student and creates a linked account used for authentication and access to the system.")]
    [EndpointName("CreateStudentAccount")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.SensitiveRateLimmitingPolicyName)]
    public async Task<ActionResult<StudentDto>> CreateStudent([FromForm] CreateStudentRequest request, CancellationToken ct)
    {
        //Upload Image, Get Image Name 
        var ImageUploadResult = await imageHelper.UploadImage(request.PersonalPhoto, ApiConstants.StudentImagesUploadFolderPath);

        if (ImageUploadResult.IsFailure)
            return Problem(ImageUploadResult.Errors, HttpContext);

        var command = new CreateStudentCommand(
            request.Name,
            request.UniversityMailAddress,
            ImageUploadResult.Value,
            request.Password,
            request.PhoneNumber,
            request.TelegramUserId);

        var result = await sender.Send(command, ct);

        if (result.IsFailure)
            await imageHelper.DeleteImage(ImageUploadResult.Value, ApiConstants.StudentImagesUploadFolderPath);


        return result.Match(
           studentDto => CreatedAtRoute(
               routeName: "GetStudentById",
               routeValues: new { version = "1.0", id = studentDto.Id },
               value: studentDto),

           e => Problem(e, HttpContext));
    }


    [HttpPatch("{id:guid}")]
    [Authorize(Policy = PoliciesNames.StudentOwnershipPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Update a student Informations.")]
    [EndpointDescription("Update Student Data By Id.")]
    [EndpointName("UpdateStudent")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.SensitiveRateLimmitingPolicyName)]
    public async Task<ActionResult> UpdateStudent([FromRoute] Guid id, [FromBody] UpdateStudentRequest request, CancellationToken ct)
    {
        var result = await sender.Send(
            new UpdateStudentCommand(
                id,
            request.Name),
            ct);

        return result.Match(
           response => NoContent(),
           e => Problem(e, HttpContext));
    }

}