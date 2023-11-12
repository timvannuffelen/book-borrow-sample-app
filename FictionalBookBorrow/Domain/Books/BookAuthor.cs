using Ardalis.GuardClauses;
using Domain.Common;

namespace Domain.Books;

/// <summary>
/// Models the intermediate table that is used on the database level to implement the many-to-many relationship
/// between a <see cref="Book"/> and an <see cref="Author"/>.
/// </summary>
/// <remarks>
/// Because the intermediate table contains other information besides the foreign keys to the related tables, we
/// introduce a class for it. Note that when the intermediate table contains only contains references (foreign
/// keys) to the related tables, we would not need to introduce a class for it.
/// Learn more: https://enterprisecraftsmanship.com/posts/modeling-relationships-in-ddd-way/
/// </remarks>
public class BookAuthor : EntityBase<int>
{
    public Book Book { get; }
    public Author Author { get; }
    public DateTime DateAdded { get; }

    public BookAuthor(Book book, Author author, DateTime dateAdded)
    {
        Book = Guard.Against.Null(book);
        Author = Guard.Against.Null(author);
        DateAdded = dateAdded;
    }
}
