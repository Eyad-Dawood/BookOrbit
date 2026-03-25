namespace BookOrbit.Application.Features.Students.Commands.BanStudent;
public class BanStudentCommandValidator : AbstractValidator<BanStudentCommand>
{
    public BanStudentCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .Cascade(CascadeMode.Stop)
            .IdRules();
    }
}