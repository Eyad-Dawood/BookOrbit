
namespace BookOrbit.Application.Features.Students.Dtos;

public class StudentListItemDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = null;
    public string? TelegramUserId { get; set; } = null;
    public string UniversityMailAddress { get; set; } = string.Empty;
    public string PersonalPhotoUrlAddress { get; set; } = string.Empty;
    public int Points { get; set; }
    public StudentState State { get; set; }

    private StudentListItemDto(
  Guid id,
  string name,
  string? phoneNumber,
  string? telegramUserId,
  string universityMailAddress,
  string personalPhotoUrlAddress,
  int points,
  StudentState state)
    {
        Id = id;
        Name = name;
        PhoneNumber = phoneNumber;
        TelegramUserId = telegramUserId;
        UniversityMailAddress = universityMailAddress;
        PersonalPhotoUrlAddress = personalPhotoUrlAddress;
        Points = points;
        State = state;
    }

    public static Expression<Func<Student, StudentListItemDto>> Projection =>
    s => new StudentListItemDto(
        s.Id,
        s.Name.Value,
        s.PhoneNumber != null ? s.PhoneNumber.Value : null,
        s.TelegramUserId != null ? s.TelegramUserId.Value : null,
        s.UniversityMail.Value,
        s.PersonalPhotoUrl.Value,
        s.Points,
        s.State
    );

}
