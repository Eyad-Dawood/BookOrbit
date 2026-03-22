namespace BookOrbit.Api.Controllers;

[Route("api/v{version:apiVersion}/images")]
[ApiController]
[Authorize]
public class ImagesController
    (ImageHelper imageHelper): ApiController
{

    [HttpGet("students/{fileName}")]
    [Authorize(Policy = PoliciesNames.ActiveUsersPolicy)]
    [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves a student's profile image.")]
    [EndpointDescription("Returns the student's profile image by file name. If the image is not found, a default image is returned.")]
    [MapToApiVersion("1.0")]
    [EndpointName("GetStudentImage")]
    [EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
    //[ResponseCache(Duration = ApiConstants.ImagesResponseCacheDurationInSeconds, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult> GetStudentImage([FromRoute]string fileName)
    {
        fileName = Path.GetFileName(fileName);
        var extension = Path.GetExtension(fileName).ToLower();

        if (!ImageHelper.IsValidImageExtension(extension))
            return BadRequest("UnSupported Image Extension");


        var contentType = extension switch
        {
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };


        var image = await imageHelper.GetStudentImage(fileName);

        if (image == null)
            return NotFound();


        return File(image, contentType);
    }

}