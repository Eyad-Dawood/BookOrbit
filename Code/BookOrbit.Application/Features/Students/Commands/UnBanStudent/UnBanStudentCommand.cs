namespace BookOrbit.Application.Features.Students.Commands.UnBanStudent;
public record UnBanStudentCommand(Guid StudentId):IRequest<Result<Updated>>;