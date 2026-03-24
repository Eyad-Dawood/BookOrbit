using BookOrbit.Application.Features.Students.Queries.GetStudentPersonalPhotoFileNameById;

namespace BookOrbit.Api.Controllers;

[Route("api/v{version:apiVersion}/images")]
[ApiController]
[Authorize]
public class ImagesController
    (ImageHelper imageHelper,
    ISender sender): ApiController
{

    [HttpGet("students/{studentId:guid}")]
    [Authorize(Policy = PoliciesNames.RegisteredUserPolicy)]
    [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves a student's profile image.")]
    [EndpointDescription("Returns the student's profile image by student Id. If the image is not found, a default image is returned.")]
    [MapToApiVersion("1.0")]
    [EndpointName("GetStudentImage")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    [ResponseCache(Duration = ApiConstants.ImagesResponseCacheDurationInSeconds, Location = ResponseCacheLocation.Client)] // Store in browser
    public async Task<ActionResult> GetStudentImage([FromRoute]Guid studentId)
    {
        var fileNameResult = await sender.Send(new GetStudentPersonalPhotoFileNameByIdQuery(
            studentId));

        if (fileNameResult.IsFailure)
            return Problem(fileNameResult.Errors, HttpContext);


        var extension = Path.GetExtension(fileNameResult.Value).ToLower();

        if (!ImageHelper.IsValidImageExtension(extension))
            return BadRequest("UnSupported Image Extension");

        var contentType = extension switch
        {
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };

        var image = await imageHelper.GetStudentImage(fileNameResult.Value);

        if (image == null)
            return NotFound();


        return File(image, contentType);
    }

}