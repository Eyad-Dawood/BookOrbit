
namespace BookOrbit.Application.Features.Students.Queries.GetStudents;

public record GetStudentsQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    string? SortColumn = "createdAt",
    string? SortDirection = "desc") : ICachedQuery<Result<PaginatedList<StudentListItemDto>>>
{
    public string CacheKey => StudentCachingConstants.StudentListKey(this);

    public string[] Tags => [StudentCachingConstants.StudentTag];

    public TimeSpan Expiration => TimeSpan.FromMinutes(StudentCachingConstants.ExpirationInMinutes);
}