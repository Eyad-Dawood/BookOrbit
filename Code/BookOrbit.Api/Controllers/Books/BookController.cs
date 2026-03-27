namespace BookOrbit.Api.Controllers.Books;

[Route("api/v{version:apiVersion}/books")]
[ApiVersion("1.0")]
[Authorize]

public class BookController(
    ISender sender,
    ImageHelper imageHelper) : ApiController
{
    [HttpPost]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Create a new book .")]
    [EndpointDescription("Create a new book to the system.")]
    [EndpointName("CreateBook")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<BookDto>> CreateBook([FromForm] CreateBookRequest request,CancellationToken ct)
    {
        //Upload Image, Get Image Name 
        var ImageUploadResult = await imageHelper.UploadImage(request.CoverImage, ApiConstants.BookCoverImagesUploadFolderPath);

        if (ImageUploadResult.IsFailure)
            return Problem(ImageUploadResult.Errors, HttpContext);

        BookCategory? category = FlagEnumHelper.Map(request.Categories);

        //If null , use none
        category ??= BookCategory.None;


        var command = new CreateBookCommand(
            request.Title,
            request.ISBN,
            request.Publisher,
            category.Value,
            request.Author,
            ImageUploadResult.Value);

        var result = await sender.Send(command, ct);
        if (result.IsFailure)
            await imageHelper.DeleteImage(ImageUploadResult.Value, ApiConstants.BookCoverImagesUploadFolderPath);

         return result.Match(
            bookDto => CreatedAtRoute(
                routeName: "GetBookById",
                routeValues: new { version = "1.0", id = bookDto.Id },
                value: bookDto),
        
            e => Problem(e, HttpContext));
    }


    [HttpPatch("{id:guid}")]
    [Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Update a book Informations.")]
    [EndpointDescription("Update book Data By Id.")]
    [EndpointName("UpdateBook")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult> UpdateBook([FromRoute] Guid id, [FromBody] UpdateBookRequest request, CancellationToken ct)
    {
        var result = await sender.Send(
            new UpdateBookCommand(
                id,
            request.Title),
            ct);

        return result.Match(
           response => NoContent(),
           e => Problem(e, HttpContext));
    }



    [HttpGet("{id:guid}", Name = "GetBookById")]
    [Authorize(Policy = PoliciesNames.ActiveStudentPolicy)]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a book by ID.")]
    [EndpointDescription("Returns detailed information about the specified book if found.")]
    [EndpointName("GetBookById")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<BookDto>> GetBookById([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetBookByIdQuery(id);

        var result = await sender.Send(query, ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }



    [HttpGet]
    [Authorize(Policy = PoliciesNames.ActiveStudentPolicy)]
    [ProducesResponseType(typeof(PaginatedList<BookListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesDefaultResponseType]
    [EndpointSummary("Retrieves a paginated list of books.")]
    [EndpointDescription("Supports filtering by searching by term and category. Pagination and sorting are supported.")]
    [MapToApiVersion("1.0")]
    [EndpointName("GetBooks")]
    [OutputCache(PolicyName = ApiConstants.DefaultOutputCachePolicyName)]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    public async Task<ActionResult<BookDto>> GetBooks([FromQuery] BookPagedFilterRequest request,CancellationToken ct)
    {
        var query = new GetBooksQuery(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.SortColumn,
            request.SortDirection,
            request.Categories);

        var result = await sender.Send(query, ct);

        return result.Match(
           Ok,
           e => Problem(e, HttpContext));
    }
}
