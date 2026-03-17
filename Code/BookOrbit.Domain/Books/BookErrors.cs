namespace BookOrbit.Domain.Books;

static public class BookErrors
{
    private const string ClassName = nameof(Book);
    static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName,"Id","Id");
    static public readonly Error TitleRequired = CommonErrors.RequiredProp(ClassName,"Title","Title");
    static public readonly Error InvalidTitle = CommonErrors.InvalidProp(ClassName, "Title", "Title", $"It must be between {BookValidationConstants.TitleMinLength} and {BookValidationConstants.TitleMaxLength} characters");
    static public readonly Error PublisherRequired = CommonErrors.RequiredProp(ClassName,"Publisher","Publisher");
    static public readonly Error InvalidPublisher = CommonErrors.InvalidProp(ClassName, "Publisher", "Publisher", $"It must be between {BookValidationConstants.PublisherMinLength} and {BookValidationConstants.PublisherMaxLength} characters");
    static public readonly Error AuthorRequired = CommonErrors.RequiredProp(ClassName,"Author","Author");
    static public readonly Error InvalidAuthor = CommonErrors.InvalidProp(ClassName, "Author", "Author", $"It must be between {BookValidationConstants.AuthorMinLength} and {BookValidationConstants.AuthorMaxLength} characters");
    static public readonly Error InvalidCategory = CommonErrors.InvalidProp(ClassName, "BookCategory", "Book Category", $"Invalid category value");
    #region ISBN
    static public readonly Error ISBNRequired = CommonErrors.RequiredProp(ClassName,"ISBN","ISBN");
    static public readonly Error InvalidISBN = CommonErrors.InvalidProp(ClassName, "ISBN", "ISBN", "It must be a valid ISBN-10 or ISBN-13 format");
    #endregion
}

