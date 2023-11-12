using Ardalis.GuardClauses;
using Domain.Common;

namespace Domain.Books;

/// <summary>
/// Represents an author of a book.
/// </summary>
public sealed class Author : EntityBase<int>
{
    private readonly IList<BookAuthor> _bookAuthors = new List<BookAuthor>();

    public string FirstName { get; }
    public string LastName { get; }


    public Author(string firstName, string lastName)
    {
        FirstName = Guard.Against.NullOrEmpty(firstName);
        LastName = Guard.Against.NullOrEmpty(lastName);
    }

    /// <summary>
    /// Get all books that are associated with this author.
    /// </summary>
    /// <remarks>
    /// We're using a method, instead of a property because we are returning a copy of the
    /// <see cref="_bookAuthors"/> list.
    /// Learn more: https://rules.sonarsource.com/csharp/RSPEC-2365/
    /// </remarks>
    public IReadOnlyList<Book> GetBooks() => _bookAuthors.OrderBy(x => x.DateAdded)
                                                         .Select(x => x.Book)
                                                         .ToList();

    public void AddBook(Book book)
    {

    }

    /// <remarks>
    /// Note that we keep this method <em>internal</em> because the <see cref="Book"/> class is the owner of the
    /// many-to-many relationship between books and authors. Marking this method as internal makes this requirement
    /// more explicit. Also, we don't want clients to be able to add a book to an author directly. They should,
    /// instead, add an author to a book.
    /// </remarks>
    internal void AddBook(BookAuthor bookAuthor)
    {
        _bookAuthors.Add(bookAuthor);
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}
