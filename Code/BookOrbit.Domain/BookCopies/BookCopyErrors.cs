namespace BookOrbit.Domain.BookCopies
{
    static public class BookCopyErrors
    {
        private const string ClassName = nameof(BookCopy);

        static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName, "Id", "Id");
        static public readonly Error OwnerIdRequired = CommonErrors.RequiredProp(ClassName, "OwnerId", "Owner Id");
        static public readonly Error BookIdRequired = CommonErrors.RequiredProp(ClassName, "BookId", "Book Id");
        static public readonly Error InvalidCondition = CommonErrors.InvalidProp(ClassName, "BookCopyCondition", "Book Copy Condition", $"Invalid condition value");
    }
}