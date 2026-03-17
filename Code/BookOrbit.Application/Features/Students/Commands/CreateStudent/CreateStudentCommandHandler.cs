using BookOrbit.Domain.Students.ValueObjects;

namespace BookOrbit.Application.Features.Students.Commands.CreateStudent;

public class CreateStudentCommandHandler(
    ILogger<CreateStudentCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache,
    IImageService imageService,
    IFormatService formatService)
    : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
{
    public async Task<Result<StudentDto>> Handle(CreateStudentCommand command, CancellationToken ct)
    {
        var email = UniversityMail.Normalize(command.UniversityMailAddress);

        var emailExists = await context.Students.AnyAsync(
            s => s.UniversityMail == email, ct);

        if (emailExists)
        {
            logger.LogWarning("Student creation aborted. Email already exists. Email:{Email}", formatService.MaskEmail(email));

            return StudentErrors.StudentEmailExists;
        }

        if (!string.IsNullOrWhiteSpace(command.TelegramUserId))
        {
            var telegramUserId = TelegramUserId.Normalize(command.TelegramUserId);

            var telegramUserIdExists = await context.Students.AnyAsync(
                s => s.TelegramUserId != null
                && s.TelegramUserId == telegramUserId, ct);

            if (telegramUserIdExists)
            {
                logger.LogWarning("Student creation aborted. Telegram user ID already exists.");
                return StudentErrors.StudentTelegramIdExists;
            }
        }

        if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
        {
            var phoneNumber = PhoneNumber.Normalize(command.PhoneNumber);

            var phoneNumberExists = await context.Students.AnyAsync(
                 s => s.PhoneNumber != null && s.PhoneNumber == phoneNumber, ct);

            if (phoneNumberExists)
            {
                logger.LogWarning("Student creation aborted. Phone number already exists.");
                return StudentErrors.StudentPhoneNumberExists;
            }
        }

        var imageUrlResult = await imageService.GetImageUrlById(command.PersonalImageId, ct);

        if (imageUrlResult.IsFailure)
        {
            logger.LogWarning("Student creation aborted. Personal image not found.");
            return StudentErrors.PersonalImageNotFound;
        }

        var Id = Guid.NewGuid();
        var createdStudentResult = Student.Create(
            id: Id,
            name: command.Name,
            universityMailAddress: command.UniversityMailAddress,
            personalPhotoUrlAddress: imageUrlResult.Value,
            phoneNumber: command.PhoneNumber,
            telegramUserId: command.TelegramUserId);

        if (createdStudentResult.IsFailure)
        {
            return createdStudentResult.Errors;
        }

        context.Students.Add(createdStudentResult.Value);

        await context.SaveChangesAsync(ct);

        await cache.RemoveByTagAsync(CachingConstants.StudentTag, ct);

        logger.LogInformation("Student created successfully with ID: {StudentId}", createdStudentResult.Value.Id);

        return StudentDto.FromEntity(createdStudentResult.Value);
    }
}

