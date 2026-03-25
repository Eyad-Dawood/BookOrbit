namespace BookOrbit.Application.Features.Students.Commands.RejectStudent;
public record RejectStudentCommand(Guid StudentId):IRequest<Result<Updated>>;