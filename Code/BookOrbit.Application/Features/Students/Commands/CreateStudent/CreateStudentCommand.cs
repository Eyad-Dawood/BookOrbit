namespace BookOrbit.Application.Features.Students.Commands.CreateStudent;

public record CreateStudentCommand(
        string Name,
        string UniversityMailAddress,
        string PersonalPhotoUrl,
        string Password,
        string? PhoneNumber = null,
        string? TelegramUserId = null)
    :IRequest<Result<StudentDto>>;