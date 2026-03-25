namespace BookOrbit.Application.Features.Students.Commands.ApproveStudent;
public record ApproveStudentCommand(Guid StudentId) : IRequest<Result<Updated>>;