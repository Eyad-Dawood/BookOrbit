namespace BookOrbit.Domain.LendingListings;

static public class LendingListRecordErrors
{
    private const string ClassName = nameof(LendingListRecord);

    static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName, "Id", "Id");
    static public readonly Error BookCopyIdRequired = CommonErrors.RequiredProp(ClassName, "BookCopyId", "Book Copy Id");
    static public readonly Error InvalidBorrowingDuration = CommonErrors.InvalidProp(ClassName, "BorrowingDurationInDays", "Borrowing Duration In Days", $"It must be between {LendingListValidationConstants.MinBorrowingDurationInDays} and {LendingListValidationConstants.MaxBorrowingDurationInDays} days.");
    static public readonly Error InvalidCostInPoints = CommonErrors.InvalidProp(ClassName, "CostInPoints", "Cost In Points", $"It must be between {LendingListValidationConstants.MinCostInPoints} and {LendingListValidationConstants.MaxCostInPoints} points.");
    static public readonly Error InvalidExpirationDate = CommonErrors.DateShouldBeInFuture(ClassName, "ExpirationDate", "Expiration Date");

}
