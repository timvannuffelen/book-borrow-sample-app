using Domain.Books;

namespace Domain.Books;

public interface IBorrowStrategy
{
    /// <summary>
    /// Returns the number of <em>days</em> that a book can be borrowed.
    /// </summary>
    int GetBorrowDuration(Book book);
}
