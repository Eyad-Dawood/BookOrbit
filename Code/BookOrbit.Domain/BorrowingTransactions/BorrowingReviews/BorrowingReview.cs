namespace BookOrbit.Domain.BorrowingTransactions.BorrowingReviews;

public class BorrowingReview : AuditableEntity
{
    public Guid ReviewerStudentId { get; }
    public Guid ReviewedStudentId { get; }
    public Guid BorrowingTransactionId { get; }

    public string? Description { get; }
    public int Rating { get; }

    private BorrowingReview() { }

    private BorrowingReview(
        Guid id,
        Guid reviewerStudentId,
        Guid reviewedStudentId,
        Guid borrowingTransactionId,
        string? description,
        int rating) : base(id)
    {
        ReviewerStudentId = reviewerStudentId;
        ReviewedStudentId = reviewedStudentId;
        BorrowingTransactionId = borrowingTransactionId;
        Description = string.IsNullOrWhiteSpace(description)?
            null:
            Description;
        Rating = rating;
    }

    static public Result<BorrowingReview> Create(
        Guid id,
        Guid reviewerStudentId,
        Guid reviewedStudentId,
        Guid borrowingTransactionId,
        string? description,
        int rating)
    {
        if (id == Guid.Empty)
            return BorrowingReviewErrors.IdRequired;

        if (reviewerStudentId == Guid.Empty)
            return BorrowingReviewErrors.ReviewerStudentIdRequired;

        if (reviewedStudentId == Guid.Empty)
            return BorrowingReviewErrors.ReviewedStudentIdRequired;

        if (borrowingTransactionId == Guid.Empty)
            return BorrowingReviewErrors.BorrowingTransactionIdRequired;

        if(rating < BorrowingReviewValidationConstants.MinRating || rating > BorrowingReviewValidationConstants.MaxRating)
            return BorrowingReviewErrors.InvalidRating;


        return new BorrowingReview(
            id,
            reviewerStudentId,
            reviewedStudentId,
            borrowingTransactionId,
            description,
            rating);
    }
}

