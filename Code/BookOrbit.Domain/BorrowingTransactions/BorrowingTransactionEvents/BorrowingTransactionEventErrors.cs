namespace BookOrbit.Domain.BorrowingTransactions.BorrowingTransactionEvents;

static public class BorrowingTransactionEventErrors
{
    private const string ClassName = nameof(BorrowingTransactionEvent);
    static public readonly Error BorrowingTransactionIdRequired = CommonErrors.RequiredProp(ClassName, "BorrowingTransactionId", "Borrowing Transaction Id");
    static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName, "Id", "Id");
    static public readonly Error InvalidState = CommonErrors.InvalidProp(ClassName, "State","State","Invalid state value");

}
