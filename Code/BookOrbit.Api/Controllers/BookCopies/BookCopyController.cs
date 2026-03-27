namespace BookOrbit.Api.Controllers.BookCopies;

[Route("api/v{version:apiVersion}")]
[ApiVersion("1.0")]
[Authorize]
public class BookCopyController(
    ISender sender) : ApiController
{

    [HttpPost("students/{id:guid}/books/copies")]
    [Authorize(Policy = PoliciesNames.ActiveStudentPolicy)]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Create a new book copy .")]
    [EndpointDescription("Create a new book copy to the system.")]
    [EndpointName("CreateBookCopy")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<BookCopyDtoWithBookDetails>> CreateBookCopy([FromRoute] Guid id,[FromBody] CreateBookCopyRequest request, CancellationToken ct)
    {
        var command = new CreateBookCopyCommand(
            id,
            request.BookId,
            request.Condition);

        var result = await sender.Send(command, ct);

        return result.Match(
       bookDto => CreatedAtRoute(
       routeName: "GetBookCopyById",
       routeValues: new { version = "1.0", id = bookDto.Id },
       value: bookDto),

       e => Problem(e, HttpContext));
    }


    [HttpPatch("books/copies/{id:guid}")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Update a book copy Informations.")]
    [EndpointDescription("Update book copy Data By Id.")]
    [EndpointName("UpdateBookCopy")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult> UpdateBookCopy([FromRoute] Guid id, [FromBody] UpdateBookCopyRequest request, CancellationToken ct)
    {
        var result = await sender.Send(
            new UpdateBookCopyCommand(
                id,
            request.Condition),
            ct);

        return result.Match(
           response => NoContent(),
           e => Problem(e, HttpContext));
    }



    [HttpGet("books/copies/{id:guid}", Name = "GetBookCopyById")]
    [Authorize(Policy = PoliciesNames.ActiveStudentPolicy)]
    [ProducesResponseType(typeof(BookCopyDtoWithBookDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a book copy by ID.")]
    [EndpointDescription("Returns detailed information about the specified book copy if found.")]
    [EndpointName("GetBookCopyById")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<BookCopyDtoWithBookDetails>> GetBookCopyById([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetBookCopyByIdQuery(id);

        var result = await sender.Send(query, ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }


    [HttpGet("books/{id:guid}/copies")]
    [Authorize(Policy = PoliciesNames.ActiveStudentPolicy)]
    [ProducesResponseType(typeof(PaginatedList<BookCopyListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a paginated list of book copies for a specific book if found.")]
    [EndpointDescription("Supports filtering by searching by term and state and condition. Pagination and sorting are supported.")]
    [MapToApiVersion("1.0")]
    [EndpointName("GetBookCopiesByBookId")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<BookCopyListItemDto>> GetBookCopiesByBookId([FromRoute] Guid id,[FromQuery] BookCopyPagedFilterRequest request, CancellationToken ct)
    {
        var query = new GetBookCopiesQuery(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.SortColumn,
            request.SortDirection,
            BookId : id,
            OwnerId : null,
            request.Conditions,
            request.States);

        var result = await sender.Send(query, ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }

    [HttpGet("students/{id:guid}/books/copies")]
    [Authorize(Policy = PoliciesNames.ActiveStudentPolicy)]
    [ProducesResponseType(typeof(PaginatedList<BookCopyListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a paginated list of book copies for a specific student if found.")]
    [EndpointDescription("Supports filtering by searching by term and state and condition. Pagination and sorting are supported.")]
    [MapToApiVersion("1.0")]
    [EndpointName("GetBookCopiesByStudentId")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<BookCopyListItemDto>> GetBookCopiesByStudentId([FromRoute] Guid id, [FromQuery] BookCopyPagedFilterRequest request, CancellationToken ct)
    {
        var query = new GetBookCopiesQuery(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.SortColumn,
            request.SortDirection,
            BookId: null,
            OwnerId: id,
            request.Conditions,
            request.States);

        var result = await sender.Send(query, ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }


    [HttpGet("books/copies")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedList<BookCopyListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a paginated list of book copies .")]
    [EndpointDescription("Supports filtering by searching by term and state and condition. Pagination and sorting are supported.")]
    [MapToApiVersion("1.0")]
    [EndpointName("GetBookCopies")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<BookCopyListItemDto>> GetBookCopies([FromQuery] BookCopyPagedFilterRequest request, CancellationToken ct)
    {
        var query = new GetBookCopiesQuery(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.SortColumn,
            request.SortDirection,
            BookId: null,
            OwnerId: null,
            request.Conditions,
            request.States);

        var result = await sender.Send(query, ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }

}