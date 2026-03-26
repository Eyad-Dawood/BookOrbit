namespace BookOrbit.Application.Features.Books.Commands.UpdateBook;
public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .IdRules();

        RuleFor(x=>x.Title)
            .Cascade(CascadeMode.Stop)
            .BookTitleRules();
    }
}