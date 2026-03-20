namespace BookOrbit.Api.Common.Mappers
{
    public static class ErrorToProblemMapper
    {
        public static ProblemDetails Map(List<Error>? errors)
        {
            if (errors == null || errors.Count == 0)
            {
                return new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Unknown error"
                };
            }

            if (errors.All(e => e.Type == ErrorKind.Validation))
            {
                var validationProblem = new ValidationProblemDetails(
                    errors
                        .GroupBy(e => e.Code)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.Description).ToArray()
                        )
                );

                validationProblem.Status = StatusCodes.Status400BadRequest;

                return validationProblem;
            }

            var error = errors[0];

            var statusCode = error.Type switch
            {
                ErrorKind.Conflict => StatusCodes.Status409Conflict,
                ErrorKind.Validation => StatusCodes.Status400BadRequest,
                ErrorKind.NotFound => StatusCodes.Status404NotFound,
                ErrorKind.Unauthorized => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError,
            };

            return new ProblemDetails
            {
                Status = statusCode,
                Title = error.Description,
                Type = error.Type.ToString()
            };
        }
    }
}
