namespace BookOrbit.Application.Features.Identity.Commands.ConfirmEmail;
public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(IdentityApplicationErrors.IdRequired.Description);

        RuleFor(x => x.EncodedToken)
            .NotEmpty().WithMessage(IdentityApplicationErrors.EmailConfirmationTokenRequired.Description);
    }
}
