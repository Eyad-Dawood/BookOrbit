
namespace BookOrbit.Application.Features.Students.Queries.GetStudentPersonalPhotoFileNameById;
public class GetStudentPersonalPhotoFileNameByIdQueryHandler (
    IAppDbContext context)
    : IRequestHandler<GetStudentPersonalPhotoFileNameByIdQuery, Result<string>>
{
    public async Task<Result<string>> Handle(GetStudentPersonalPhotoFileNameByIdQuery request, CancellationToken ct)
    {
        var student = await context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.StudentId,ct);

        if (student is null)
        {
            return StudentApplicationErrors.NotFoundById;
        }

        return student.PersonalPhotoFileName;
    }
}