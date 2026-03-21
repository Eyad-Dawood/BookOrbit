using System.Text.Json.Serialization;

namespace BookOrbit.Application.Features.Students.Dtos;

public class StudentDto
{

    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = null;
    public string? TelegramUserId { get; set; } = null;
    public string UniversityMailAddress { get; set; } = string.Empty;
    public string PersonalPhotoFileName { get; set; } = string.Empty;
    public int Points { get; set; }
    public StudentState State { get; set; }

    [JsonConstructor]
    private StudentDto() { }


    private StudentDto(
    Guid id,
    string name,
    string? phoneNumber,
    string? telegramUserId,
    string universityMailAddress,
    string ersonalPhotoFileName,
    int points,
    StudentState state)
    {
        Id = id;
        Name = name;
        PhoneNumber = phoneNumber;
        TelegramUserId = telegramUserId;
        UniversityMailAddress = universityMailAddress;
        PersonalPhotoFileName = ersonalPhotoFileName;
        Points = points;
        State = state;
    }

    static public StudentDto FromEntity(Student entity)
    {
        return new StudentDto(
            entity.Id,
            entity.Name.Value,
            entity.PhoneNumber?.Value,
            entity.TelegramUserId?.Value,
            entity.UniversityMail.Value,
            entity.PersonalPhotoFileName,
            entity.Points,
            entity.State);
    }

    public static Expression<Func<Student, StudentDto>> Projection =>
     s => new StudentDto(
         s.Id,
         s.Name.Value,
         s.PhoneNumber != null ? s.PhoneNumber.Value : null,
         s.TelegramUserId != null ? s.TelegramUserId.Value : null,
         s.UniversityMail.Value,
         s.PersonalPhotoFileName,
         s.Points,
         s.State
     );

}