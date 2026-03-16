
namespace BookOrbit.Domain.LendingListings;
public class LendingListRecord : ExpirableEntity
{
    public Guid BookCopyId { get; }
    public LendingListRecordState State { get; }
    public int BorrowingDurationInDays { get; }
    public int CostInPoints { get; }
    
    public BookCopy? BookCopy { get; private set; }

    private LendingListRecord() { }

    private LendingListRecord(
        Guid id,
        Guid bookCopyId,
        int borrowingDurationInDays,
        int costInPoints,
        DateTimeOffset expirationDateUtc) : base(id)
    {
        BookCopyId = bookCopyId;
        State = LendingListRecordState.Available;
        BorrowingDurationInDays = borrowingDurationInDays;
        CostInPoints = costInPoints;
        ExpirationDateUtc = expirationDateUtc;
    }

    public static Result<LendingListRecord> Create(
        Guid id,
        Guid bookCopyId,
        int borrowingDurationInDays,
        int costInPoints,
        DateTimeOffset expirationDateUtc,
        DateTimeOffset currentTime // This parameter is added to make the method more testable, as it allows us to control the current time during testing. 
        )
    {
        if (id == Guid.Empty)
            return LendingListRecordErrors.IdRequired;

        if (bookCopyId == Guid.Empty)
            return LendingListRecordErrors.BookCopyIdRequired;

        if (borrowingDurationInDays > LendingListValidationConstants.MaxBorrowingDurationInDays || borrowingDurationInDays < LendingListValidationConstants.MinBorrowingDurationInDays)
            return LendingListRecordErrors.InvalidBorrowingDuration;

        if (costInPoints > LendingListValidationConstants.MaxCostInPoints || costInPoints < LendingListValidationConstants.MinCostInPoints)
            return LendingListRecordErrors.InvalidCostInPoints;

        if (expirationDateUtc <= currentTime)
            return LendingListRecordErrors.InvalidExpirationDate;

        return new LendingListRecord(
            id,
            bookCopyId,
            borrowingDurationInDays,
            costInPoints,
            expirationDateUtc);
    }
}

