namespace BookOrbit.Application.Features.Students.Commands.BanStudent;
public record BanStudentCommand(Guid StudentId):IRequest<Result<Updated>>;