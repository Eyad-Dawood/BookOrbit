namespace BookOrbit.Domain.BorrowingTransactions.BorrowingReviews;

public class BorrowingReviewErrors
{
    private const string ClassName = nameof(BorrowingReview);

    static public readonly Error IdRequired = CommonErrors.RequiredProp(ClassName, "Id", "Id");
    static public readonly Error ReviewerStudentIdRequired = CommonErrors.RequiredProp(ClassName, "ReviewerStudentId", "Reviewer Student Id");
    static public readonly Error ReviewedStudentIdRequired = CommonErrors.RequiredProp(ClassName, "ReviewedStudentId", "Reviewed Student Id");
    static public readonly Error BorrowingTransactionIdRequired = CommonErrors.RequiredProp(ClassName, "BorrowingTransactionId", "Borrowing Transaction Id");
    static public readonly Error InvalidRating = CommonErrors.InvalidProp(ClassName, "Rating", "Rating", $"It must be between {BorrowingReviewValidationConstants.MinRating} and {BorrowingReviewValidationConstants.MaxRating}");

}