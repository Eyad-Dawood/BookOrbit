namespace BookOrbit.Application.Contracts.Requests.Students;

public class CreateStudentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = null;
    public string? TelegramUserId { get; set; } = null;
    public string UniversityMailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid PersonalImageId { get; set; } = Guid.Empty;
}