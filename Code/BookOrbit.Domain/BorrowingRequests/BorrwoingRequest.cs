namespace BookOrbit.Domain.BorrowingRequests;

public class BorrwoingRequest : ExpirableEntity
{
    public Guid BorrowingStudentId { get; }
    public Guid LendingRecordId { get; }
    public BorrowingRequestState State { get; }

    private BorrwoingRequest(){ }
    private BorrwoingRequest(
        Guid id, 
        Guid borrowingStudentId, 
        Guid lendingRecordId,
        DateTimeOffset expirationDateUtc) : base(id)
    {
        BorrowingStudentId = borrowingStudentId;
        LendingRecordId = lendingRecordId;
        State = BorrowingRequestState.Pending;
        ExpirationDateUtc = expirationDateUtc;
    }

    static public Result<BorrwoingRequest> Create(
        Guid id,
        Guid borrowingStudentId,
        Guid lendingRecordId,
        DateTimeOffset expirationDateUtc,
        DateTimeOffset currentTime // This parameter is added to make the method more testable, as it allows us to control the current time during testing. 
        )
    {
        if (id == Guid.Empty)
            return BorrowingRequestErrors.IdRequired;

        if (borrowingStudentId == Guid.Empty)
            return BorrowingRequestErrors.BorrowingStudentIdRequired;

        if (lendingRecordId == Guid.Empty)
            return BorrowingRequestErrors.LendingRecordIdRequired;
       
        if (expirationDateUtc <= currentTime)
            return BorrowingRequestErrors.InvalidExpirationDate;
       
        var borrowingRequest = new BorrwoingRequest(
            id,
            borrowingStudentId,
            lendingRecordId,
            expirationDateUtc);

        return borrowingRequest;
    }
}