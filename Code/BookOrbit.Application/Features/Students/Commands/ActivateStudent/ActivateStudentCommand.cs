namespace BookOrbit.Application.Features.Students.Commands.ActivateStudent;
public record ActivateStudentCommand(Guid StudentId):IRequest<Result<Updated>>;