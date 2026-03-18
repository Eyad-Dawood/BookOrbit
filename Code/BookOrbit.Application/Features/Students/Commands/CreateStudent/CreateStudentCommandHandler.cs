
namespace BookOrbit.Application.Features.Students.Commands.CreateStudent;

public class CreateStudentCommandHandler(
    ILogger<CreateStudentCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache,
    IImageService imageService,
    IMaskingService maskingService)
    : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
{
    public async Task<Result<StudentDto>> Handle(CreateStudentCommand command, CancellationToken ct)
    {
        var emailResult = await EnsureEmailIsValidAndUnique(command.UniversityMailAddress, ct);
        if (emailResult.IsFailure)
            return emailResult.Errors;


        TelegramUserId? telegramUserId = null;
        if(!string.IsNullOrWhiteSpace(command.TelegramUserId))
        {
            var telegramUserIdResult =
            await EnsureTelegramIdIsValidAndUnique(command.TelegramUserId, ct);

            if(telegramUserIdResult.IsFailure)
                return telegramUserIdResult.Errors;

            telegramUserId = telegramUserIdResult.Value;
        }


        PhoneNumber? phoneNumber = null;
        if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
        {
            var phoneNumberResult = 
                await EnsurePhoneNumberIsValidAndUnique(command.PhoneNumber, ct);

            if (phoneNumberResult.IsFailure)
                return phoneNumberResult.Errors;

            phoneNumber = phoneNumberResult.Value;
        }


        var imageUrlFindingResult = await imageService.GetImageUrlById(command.PersonalImageId, ct);
        if (imageUrlFindingResult.IsFailure)
        {
            logger.LogWarning("Student creation aborted. Personal image not found.");
            return StudentApplicationErrors.PersonalImageNotFound;
        }

        var imageUrlCreationResult = Url.Create(imageUrlFindingResult.Value);

        if (imageUrlCreationResult.IsFailure)
            return imageUrlCreationResult.Errors;



        var nameCreationResult = StudentName.Create(command.Name);

        if(nameCreationResult.IsFailure)
            return nameCreationResult.Errors;


        var createdStudentResult = Student.Create(
            id: Guid.NewGuid(),
            name: nameCreationResult.Value,
            universityMail: emailResult.Value,
            personalPhotoUrl: imageUrlCreationResult.Value,
            phoneNumber: phoneNumber,
            telegramUserId: telegramUserId);

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

    public async Task<Result<UniversityMail>> EnsureEmailIsValidAndUnique(string email, CancellationToken ct)
    {
        var emailResult = UniversityMail.Create(email);

        if (emailResult.IsFailure)
            return emailResult.Errors;

        var emailExists = await context.Students.AnyAsync(
            s => s.UniversityMail == emailResult.Value, ct);

        if (emailExists)
        {
            logger.LogWarning(
                "Student creation failed. Reason: {Reason}, Value: {Value}",
                "Email Exists",
                maskingService.MaskEmail(email));
            return StudentApplicationErrors.EmailAlreadyExists;
        }

        return emailResult;
    }
    public async Task<Result<TelegramUserId>> EnsureTelegramIdIsValidAndUnique(string telegrameUserId, CancellationToken ct)
    {
        var telegramUserIdResult = TelegramUserId.Create(telegrameUserId);

        if (telegramUserIdResult.IsFailure)
            return telegramUserIdResult.Errors;

        var telegramUserIdExists = await context.Students.AnyAsync(
            s => s.TelegramUserId != null
            && s.TelegramUserId == telegramUserIdResult.Value, ct);

        if (telegramUserIdExists)
        {
            logger.LogWarning(
               "Student creation failed. Reason: {Reason}, Value: {Value}",
               "Telegramuser ID Exists",
              maskingService.MaskTelegramUserId(telegramUserIdResult.Value));

            return StudentApplicationErrors.TelegramUserIdAlreadyExists;
        }

        return telegramUserIdResult;
    }
    public async Task<Result<PhoneNumber>> EnsurePhoneNumberIsValidAndUnique(string phoneNumber, CancellationToken ct)
    {
        var phoneNumberResult = PhoneNumber.Create(phoneNumber);

        if (phoneNumberResult.IsFailure)
            return phoneNumberResult.Errors;

        var phoneNumberExists = await context.Students.AnyAsync(
            s => s.PhoneNumber != null
            && s.PhoneNumber == phoneNumberResult.Value, ct);

        if (phoneNumberExists)
        {
            logger.LogWarning(
               "Student creation failed. Reason: {Reason}, Value: {Value}",
               "Phone Number Exists",
              maskingService.MaskPhoneNumber(phoneNumberResult.Value));

            return StudentApplicationErrors.PhoneNumberAlreadyExists;
        }
        return phoneNumberResult;
    }
}