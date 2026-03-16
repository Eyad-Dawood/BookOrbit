namespace BookOrbit.Domain.BookCopies
{
    static public class BookCopyErrors
    {
        private const string ClassName = nameof(BookCopy);

        static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName, "Id", "Id");
        static public readonly Error OwnerIdRequired = CommonErrors.RequiredProp(ClassName, "OwnerId", "Owner Id");
        static public readonly Error BookIdRequired = CommonErrors.RequiredProp(ClassName, "BookId", "Book Id");
        static public readonly Error InvalidCondition = CommonErrors.InvalidProp(ClassName, "Condition", "Condition", $"It must be one of the following: {string.Join(", ", Enum.GetNames(typeof(BookCopyCondition)))}");
    }
}