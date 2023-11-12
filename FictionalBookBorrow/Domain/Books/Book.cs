using Domain.Common;

namespace Domain.Books;

/// <summary>
/// An abstract class that acts as the base class for each book.
/// </summary>
/// <remarks>
/// A book can be associated with multiple authors and, vice versa, an author can be associated with
/// multiple books. This means a many-to-many relationship exists between a book and an author.
/// In our domain model one of the two entities needs to be the owner of the relationship. The owner
/// is the entry point for setting up links between the two entities. The owner is responsible for
/// avoiding inconsistencies in the relationship, where the relationship is set up on one side but
/// not on the other entity. 
/// </remarks>
public abstract class Book : EntityBase<int>
{
    // Use the strategy pattern described in Object Oriented Analysis & Design to determine how long a book can be borrowed.
    // In this approach we pass a Book reference to the strategy object so the reference cna callback to the Book to do it's
    // work. This approach does not use different subclasses that represent different Book types to determine the borrow duration.
    // With this the used strategy doesn't change based on the type of the Book, but rather by time. One moment Strategy A is 
    // active, the next moment Strategy B is used. The current strategy applies to *all* book types. There is no different 
    // strategy for different book types.

    // Use the strategy pattern described in Head First Design Patterns to determine the sale price of a book.
    // In this approach we initialize each Book subclass with a strategy object that knows how to determine the sale price.
    // With this approach the strategy to use is not determined by time, but by the type of the Book. One type of Book (Subclass A)
    // uses a certain strategy, while another type of Book (Subclass B) uses a different strategy. These strategies can also 
    // change over time, but the main difference with the previous approach is that the strategy is determined by the type of the
    // Book and each book type has it's own strategy.

    private readonly IList<BookAuthor> _bookAuthors = new List<BookAuthor>();


    /// <summary>
    /// Get all authors that are associated with this book.
    /// </summary>
    /// <remarks>
    /// We're using a method, instead of a property because we are returning a copy of the
    /// <see cref="_bookAuthors"/> list.
    /// Learn more: https://rules.sonarsource.com/csharp/RSPEC-2365/
    /// </remarks>
    public IList<Author> GetAuthors() => _bookAuthors.OrderBy(x => x.DateAdded)
                                                     .Select(x => x.Author)
                                                     .ToList();

    /// <remarks>
    /// Because <see cref="Book"/> is the owner of the many-to-many relationship between books and authors,
    /// this method is responsible for setting up the bi-directional relationship between the book and the
    /// author. This method makes sure that both sides of the relationship are set up correctly.
    /// </remarks>
    public void AddAuthor(Author author)
    {
        BookAuthor bookAuthor = new(this, author, DateTime.UtcNow);
        _bookAuthors.Add(bookAuthor); // This book needs a reference to the given author.
        author.AddBook(bookAuthor); // The given author needs a reference to this book.
    }
}
