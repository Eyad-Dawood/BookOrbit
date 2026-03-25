namespace BookOrbit.Api.Controllers.Students;

[Route("api/v{version:apiVersion}/students")]
[ApiVersion("1.0")]
[Authorize]
public class StudentQueryController(
    ISender sender) : ApiController
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
    public async Task<ActionResult<PaginatedList<StudentListItemDto>>> GetStudents([FromQuery] StudentPagedFilterRequest request, CancellationToken ct)
    {
        var query = new GetStudentsQuery(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.SortColumn,
            request.SortDirection,
            request.States,
            request.EmailConfirmed);

        var result = await sender.Send(query, ct);

        return result.Match(
            Ok,
            e => Problem(e, HttpContext));
    }



    [HttpGet("{id:guid}", Name = "GetStudentById")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a student by ID.")]
    [EndpointDescription("Returns detailed information about the specified student if found.")]
    [EndpointName("GetStudentById")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<StudentDto>> GetStudentById([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetStudentByIdQuery(id);

        var result = await sender.Send(query, ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }

}