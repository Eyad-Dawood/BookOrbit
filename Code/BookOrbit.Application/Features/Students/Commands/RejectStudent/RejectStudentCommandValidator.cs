namespace BookOrbit.Application.Features.Students.Commands.RejectStudent;
public class RejectStudentCommandValidator : AbstractValidator<RejectStudentCommand>
{
    public RejectStudentCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .Cascade(CascadeMode.Stop)
            .IdRules();
    }
}
