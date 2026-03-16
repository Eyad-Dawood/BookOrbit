namespace BookOrbit.Domain.BorrowingTransactions;

static public class BorrowingTransactionErrors
{
    private const string ClassName = nameof(BorrowingTransaction);

    #region General
    static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName, "Id", "Id");
    static public readonly Error BorrowingRequestIdRequired = CommonErrors.RequiredProp(ClassName, "BorrowingRequestId", "Borrowing Request Id");
    static public readonly Error LenderStudentIdRequired = CommonErrors.RequiredProp(ClassName, "LenderStudentId", "Lender Student Id");
    static public readonly Error BorrowerStudentIdRequired = CommonErrors.RequiredProp(ClassName, "BorrowerStudentId", "Borrower Student Id");
    static public readonly Error BookCopyIdRequired = CommonErrors.RequiredProp(ClassName, "BookCopyId", "BookCopyId");
    static public readonly Error ExpectedReturnDateRequired = CommonErrors.RequiredProp(ClassName, "ExpectedReturnDate", "Expected Return Date");
    static public readonly Error InvalidExpectedReturnDate = CommonErrors.DateShouldBeInFuture(ClassName, "ExpectedReturnDate", "Expected Return Date");
    static public readonly Error LenderAndBorrowerCannotBeTheSame = CommonErrors.Custom(ClassName, "LenderAndBorrowerCannotBeTheSame", "Lender and borrower cannot be the same student.");
    static public readonly Error ReturnDateShouldBeAfterCreationDate = CommonErrors.Custom(ClassName, "ReturnDateShouldBeAfterCreaionDate", "Return date should be after creation date.");
    static public readonly Error ReturnDateCannotBeInTheFuture = CommonErrors.DateCannotBeInFuture(ClassName, "ReturnDate", "Return Date");
    static public readonly Error CannotMarkOverdueWhileExpectedReturnDateNotPast = CommonErrors.Custom(ClassName, "CannotMarkOverdueWhileExcpectedReturnDateNotPast", "Cannot mark overdue while expected return date not past.");
    #endregion

    #region Logic
    public static Error InvalidStateTransition(BorrowingTransactionState currentState, BorrowingTransactionState newState) =>
     CommonErrors.InvalidStateTransition(ClassName, currentState.ToString(), newState.ToString());

    #endregion
}
