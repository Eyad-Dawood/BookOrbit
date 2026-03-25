namespace BookOrbit.Application.Features.Students.Commands.ActivateStudent;
public class ActivateStudentCommandValidator : AbstractValidator<ActivateStudentCommand>
{
    public ActivateStudentCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .Cascade(CascadeMode.Stop)
            .IdRules();
    }
}
