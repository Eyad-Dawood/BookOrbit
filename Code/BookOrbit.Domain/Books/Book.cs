namespace BookOrbit.Domain.Books;
public class Book : AuditableEntity
{
    public string Title { get; }
    public ISBN ISBN { get; }
    public string Publisher { get; }
    public BookCategory Category { get; }
    public string Author { get; }
    public string CoverImageFileName { get; }

#pragma warning disable CS8618
    private Book() { }

    private Book(
        Guid id,
        string title,
        ISBN isbn,
        string publisher,
        BookCategory category,
        string author,
        string coverImageFileName) : base(id)
    {
        Title = title;
        ISBN = isbn;
        Publisher = publisher;
        Category = category;
        Author = author;
        CoverImageFileName = coverImageFileName;
    }

    public static Result<Book> Create(
        Guid id,
        string title,
        ISBN isbn,
        string publisher,
        BookCategory category,
        string author,
        string coverImageFileName)
    {
        if (id == Guid.Empty)
            return BookErrors.IdRequired;

        if (string.IsNullOrWhiteSpace(title))
            return BookErrors.TitleRequired;


        title = title.Trim();
        if (title.Length > BookValidationConstants.TitleMaxLength || title.Length < BookValidationConstants.TitleMinLength)
            return BookErrors.InvalidTitle;

        if (string.IsNullOrWhiteSpace(publisher))
            return BookErrors.PublisherRequired;

        publisher = publisher.Trim();
        if (publisher.Length > BookValidationConstants.PublisherMaxLength || publisher.Length < BookValidationConstants.PublisherMinLength)
            return BookErrors.InvalidPublisher;

        if (string.IsNullOrWhiteSpace(author))
            return BookErrors.AuthorRequired;

        author = author.Trim();
        if (author.Length > BookValidationConstants.AuthorMaxLength || author.Length < BookValidationConstants.AuthorMinLength)
            return BookErrors.InvalidAuthor;

        if (!Enum.IsDefined(category))
            return BookErrors.InvalidCategory;


        return new Book(
            id,
            title,
            isbn,
            publisher,
            category,
            author,
            coverImageFileName);
    }
}

