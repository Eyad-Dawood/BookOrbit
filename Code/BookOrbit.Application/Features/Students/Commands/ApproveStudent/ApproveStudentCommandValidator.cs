namespace BookOrbit.Application.Features.Students.Commands.ApproveStudent;
public class ApproveStudentCommandValidator : AbstractValidator<ApproveStudentCommand>
{
    public ApproveStudentCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .Cascade(CascadeMode.Stop)
            .IdRules();
    }
}
