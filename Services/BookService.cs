using BookManagerCLI.Data;
using BookManagerCLI.Dto;
using BookManagerCLI.Models;

namespace BookManagerCLI.Services;

public class BookService
{
    private readonly AppDbContext _context;

    public BookService()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated();
    }

    public void AddBook(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public List<Book> GetBooks()
    {
        return _context.Books.ToList();
    }

    public Book? GetBookById(int id)
    {
        return _context.Books.SingleOrDefault(b => b.Id == id);
    }

    public Boolean RemoveBook(int id)
    {
        Book? book = this.GetBookById(id);
        if (book is null) return false;

        _context.Books.Remove(book);
        _context.SaveChanges();

        return true;
    }

    public List<Book> SearchBooks(string search)
    {
        return (from book in _context.Books
            where book.Title.Contains(search) &&
                  book.Author.Contains(search)
            select book).ToList();
    }

    public bool UpdateBook(int id, BookDto bookDto)
    {
        var book = _context.Books.SingleOrDefault(b => b.Id == id);
        if (book is null) return false;

        if (!string.IsNullOrEmpty(bookDto.Title)) book.Title = bookDto.Title;
        if (!string.IsNullOrEmpty(bookDto.Author)) book.Author = bookDto.Author;
        if (bookDto.PublishedYear.HasValue) book.PublishedYear = bookDto.PublishedYear.Value;
        if (!string.IsNullOrEmpty(bookDto.Description)) book.Description = bookDto.Description;

        _context.SaveChanges();
        return true;
    }



    public int GetBookCount() => _context.Books.Count();
}