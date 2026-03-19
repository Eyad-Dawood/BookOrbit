namespace BookOrbit.Application.Features.Students.Commands.CreateStudent;

public record CreateStudentCommand(
        string Name,
        string UniversityMailAddress,
        Guid PersonalImageId,
        string Password,
        string? PhoneNumber = null,
        string? TelegramUserId = null)
    :IRequest<Result<StudentDto>>;