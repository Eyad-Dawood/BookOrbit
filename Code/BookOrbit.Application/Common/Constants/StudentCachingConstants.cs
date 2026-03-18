
namespace BookOrbit.Application.Common.Constants;

static public class StudentCachingConstants
{
    public const string StudentTag = "student";
    public static string StudentKey(Guid id) => $"student:{id}";

    public static string StudentListKey(GetStudentsQuery query)
        =>
        $"students:p={query.Page}:ps={query.PageSize}" +
        $":v={query.SearchTerm ?? "-"}" +
        $":sort={query.SortColumn}:{query.SortDirection}";

    public const int ExpirationInMinutes = 10;
}

