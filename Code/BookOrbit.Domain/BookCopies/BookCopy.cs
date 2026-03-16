
namespace BookOrbit.Domain.BookCopies;
public class BookCopy : AuditableEntity
{
    public Guid OwnerId { get; }
    public Guid BookId { get; }
    public BookCopyCondition Condition { get; }


    public Student? Owner { get; set; }
    public Book? Book { get; set; }
    
    private BookCopy(){ }

    private BookCopy(
        Guid id,
        Guid ownerId,
        Guid bookId,
        BookCopyCondition condition) : base(id)
    {
        OwnerId = ownerId;
        BookId = bookId;
        Condition = condition;
    }

    public static Result<BookCopy> Create(
        Guid id,
        Guid ownerId,
        Guid bookId,
        BookCopyCondition condition)
    {
        if (id == Guid.Empty)
            return BookCopyErrors.IdRequired;

        if (ownerId == Guid.Empty)
            return BookCopyErrors.OwnerIdRequired;

        if (bookId == Guid.Empty)
            return BookCopyErrors.BookIdRequired;

        if(!Enum.IsDefined(condition))
            return BookCopyErrors.InvalidCondition;

        return new BookCopy(id, ownerId, bookId, condition);
    }
}