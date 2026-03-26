namespace BookOrbit.Api.Contracts.Requests.Students;

public class UpdateStudentRequest
{
    public string Name { get; set; } = string.Empty;
    public IFormFile PersonalPhoto { get; set; } = default!;
}