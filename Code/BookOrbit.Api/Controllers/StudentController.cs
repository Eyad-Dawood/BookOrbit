namespace BookOrbit.Api.Controllers;

[Route("api/v{version:apiVersion}/students")]
[ApiVersion("1.0")]
[Authorize]
public sealed class StudentController(
    ISender sender,
    ImageUploadHelper imageUploadHelper) : ApiController
{
    [HttpGet]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedList<StudentListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a paginated list of students.")]
    [EndpointDescription("Supports filtering by searching by term. Pagination and sorting are supported.")]
    [MapToApiVersion("1.0")]
    [EndpointName("GetStudents")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<PaginatedList<StudentListItemDto>>> Get([FromQuery] PagedFilterRequest request, CancellationToken ct)
    {
        var query = new GetStudentsQuery(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.SortColumn,
            request.SortDirection);

        var result = await sender.Send(query, ct);

        return result.Match(
            Ok,
            e => Problem(e, HttpContext));
    }



    [HttpGet("{id:guid}",Name = "GetCustomerById")]
    [Authorize(Policy = PoliciesNames.StudentOwnershipPolicy)]
    [ProducesResponseType(typeof(StudentDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a stude-nt by ID.")]
    [EndpointDescription("Returns detailed information about the specified student if found.")]
    [EndpointName("GetStudentById")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<StudentDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetStudentByIdQuery(id);

        var result = await sender.Send(query,ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }


    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Register a new student with an associated account.")]
    [EndpointDescription("Registers a student and creates a linked account used for authentication and access to the system.")]
    [EndpointName("CreateStudentAccount")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.SensitiveRateLimmitingPolicyName)]
    public async Task<ActionResult<StudentDto>> CreateStudentAccount([FromForm] CreateStudentRequest request,CancellationToken ct)
    {
        //Upload Image, Get Image Name 
        var ImageUploadResult = await imageUploadHelper.UploadImage(request.PersonalPhoto,ApiConstants.StudentImagesUploadFolderPath);

        if(ImageUploadResult.IsFailure)
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
            await imageUploadHelper.DeleteImage(ImageUploadResult.Value, ApiConstants.StudentImagesUploadFolderPath);


        return result.Match(
           studentDto=> CreatedAtRoute(
               routeName:"GetCustomerById",
               routeValues:new { version = "1.0", id = studentDto.Id},
               value: studentDto),

           e => Problem(e, HttpContext));
    }


    [HttpPut("{id:guid}")]
    [Authorize(Policy = PoliciesNames.StudentOwnershipPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [EndpointSummary("Update a student Informations.")]
    [EndpointDescription("Update Student Data By Id.")]
    [EndpointName("UpdateStudent")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.SensitiveRateLimmitingPolicyName)]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStudentRequest request,CancellationToken ct)
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
