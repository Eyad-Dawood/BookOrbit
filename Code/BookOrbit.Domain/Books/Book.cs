namespace BookOrbit.Domain.Books;
public class Book : AuditableEntity
{
    public string Title { get; }
    public ISBN ISBN { get; }
    public string Publisher { get; }
    public BookCategory Category { get; }
    public string Author { get; }
    public Url CoverImageUrl { get; }

#pragma warning disable CS8618
    private Book() { }

    private Book(
        Guid id,
        string title,
        ISBN isbn,
        string publisher,
        BookCategory category,
        string author,
        Url coverImageUrl) : base(id)
    {
        Title = title;
        ISBN = isbn;
        Publisher = publisher;
        Category = category;
        Author = author;
        CoverImageUrl = coverImageUrl;
    }

    public static Result<Book> Create(
        Guid id,
        string title,
        string isbn,
        string publisher,
        BookCategory category,
        string author,
        string coverImageUrl)
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


        var isbnResult = ISBN.Create(isbn);
        if (isbnResult.IsFailure)
            return isbnResult.Errors;

        var coverImageUrlResult = Url.Create(coverImageUrl);
        if (coverImageUrlResult.IsFailure)
            return coverImageUrlResult.Errors;


        return new Book(
            id,
            title,
            isbnResult.Value,
            publisher,
            category,
            author,
            coverImageUrlResult.Value);
    }
}

