namespace BookOrbit.Domain.BorrowingRequests;

static public class BorrowingRequestErrors
{
    private const string ClassName = nameof(BorrowingRequest);

    static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName, "Id", "Id");
    static public readonly Error BorrowingStudentIdRequired = CommonErrors.RequiredProp(ClassName, "BorrowingStudentId", "Borrowing Student Id");
    static public readonly Error LendingRecordIdRequired = CommonErrors.RequiredProp(ClassName, "LendingRecordId", "Lending Record Id");
    static public readonly Error InvalidExpirationDate = CommonErrors.DateShouldBeInFuture(ClassName, "ExpirationDate", "Expiration Date");
}
