
namespace BookOrbit.Api.Controllers;

[Route("api/students")]
public sealed class StudentController(ISender sender) : ApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<StudentListItemDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves a paginated list of students.")]
    [EndpointDescription("Supports filtering by searching by term. Pagination and sorting are supported.")]
    [EndpointName("GetStudents")]
    //[OutputCache(PolicyName = ApiConstatns.DefaultOutputCachePolicyName)]
    public async Task<IActionResult> Get([FromQuery] PagedFilterRequest request, CancellationToken ct)
    {
        var query = new GetStudentsQuery(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.SortColumn,
            request.SortDirection);

        var result = await sender.Send(query,ct);

        return result.Match(
            Ok,
            Problem);
    }
}