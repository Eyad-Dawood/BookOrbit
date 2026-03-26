namespace BookOrbit.Application.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .IdRules();


        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .StudentNameRules();


        RuleFor(x => x.personalPhotoFileName)
            .StudentPersonalImageRules();
    }
}