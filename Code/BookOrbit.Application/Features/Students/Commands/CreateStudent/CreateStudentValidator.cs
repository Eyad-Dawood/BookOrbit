namespace BookOrbit.Application.Features.Students.Commands.CreateStudent;
public sealed class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StudentErrors.NameRequired.Description)
            .MaximumLength(StudentValidationConstants.NameMaxLength).WithMessage(StudentErrors.InvalidName.Description)
            .MinimumLength(StudentValidationConstants.NameMinLength).WithMessage(StudentErrors.InvalidName.Description);

        RuleFor(x => x.UniversityMailAddress)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StudentErrors.UniversityMailRequired.Description)
            .EmailAddress().WithMessage(StudentErrors.InvalidUniversityMail.Description)
            .Must(x=>x.EndsWith(@"std.mans.edu.eg")).WithMessage(StudentErrors.InvalidUniversityMail.Description);//Simple Email check , the rest in domain

        RuleFor(x => x.PersonalImageId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StudentErrors.PersonalImageRequired.Description)
            .Must(id => id != Guid.Empty).WithMessage(StudentErrors.PersonalImageRequired.Description);
   

        RuleFor(x => x.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(PhoneNumberConstants.MinLength).WithMessage(PhoneNumberErrors.InvalidPhoneNumber.Description)
            .MaximumLength(PhoneNumberConstants.MaxLength).WithMessage(PhoneNumberErrors.InvalidPhoneNumber.Description)
            .Must(PhoneNumberCorrectPrefix).WithMessage(PhoneNumberErrors.InvalidPhoneNumber.Description)
            .Matches(@"^\d+$").WithMessage(PhoneNumberErrors.InvalidPhoneNumber.Description) // digits only
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.TelegramUserId)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(TelegramUserIdConstants.MinLength).WithMessage(TelegramUserIdErrors.InvalidTelegramUserId.Description)
            .MaximumLength(TelegramUserIdConstants.MaxLength).WithMessage(TelegramUserIdErrors.InvalidTelegramUserId.Description)
            .Matches("^[a-zA-Z0-9_]+$").WithMessage(TelegramUserIdErrors.InvalidTelegramUserId.Description) // lower chars, digits, underscores only 
            .When(x => !string.IsNullOrWhiteSpace(x.TelegramUserId));
    }

    private bool PhoneNumberCorrectPrefix(string phoneNumber)=>
         PhoneNumberConstants.Prefixes
            .Any(prefix => phoneNumber.StartsWith(prefix));
    
   
}

