namespace BookOrbit.Application.Features.Students.Dtos;

public class StudentDto
{

    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = null;
    public string? TelegramUserId { get; set; } = null;
    public string UniversityMailAddress { get; set; } = string.Empty;
    public string PersonalPhotoUrlAddress { get; set; } = string.Empty;
    public int Points { get; set; }
    public StudentState State { get; set; }

    private StudentDto(
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

    static public StudentDto FromEntity(Student entity)
    {
        return new StudentDto(
            entity.Id,
            entity.Name,
            entity.PhoneNumber?.Value,
            entity.TelegramUserId?.Value,
            entity.UniversityMail.Value,
            entity.PersonalPhotoUrl.Value,
            entity.Points,
            entity.State);
    }

}