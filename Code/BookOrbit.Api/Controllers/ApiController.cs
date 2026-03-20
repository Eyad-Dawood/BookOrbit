
using BookOrbit.Api.Common.Mappers;

namespace BookOrbit.Api.Controllers;


[ApiController]
public class ApiController : ControllerBase
{
    protected ActionResult Problem(List<Error> errors)
    {
        var problem = ErrorToProblemMapper.Map(errors);
        return StatusCode(problem.Status ?? StatusCodes.Status500InternalServerError, problem);
    }
}

