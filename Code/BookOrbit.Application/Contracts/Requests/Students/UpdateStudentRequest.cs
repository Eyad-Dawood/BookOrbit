namespace BookOrbit.Application.Contracts.Requests.Students;

public class UpdateStudentRequest
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
}