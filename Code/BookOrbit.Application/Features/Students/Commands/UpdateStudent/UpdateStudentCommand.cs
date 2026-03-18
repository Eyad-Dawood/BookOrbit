namespace BookOrbit.Application.Features.Students.Commands.UpdateStudent;

public record UpdateStudentCommand(
    Guid Id,
    string Name) : IRequest<Result<Updated>>;