using BookManagerCLI.Dto;
using BookManagerCLI.Models;
using BookManagerCLI.Services;
using Spectre.Console;

namespace BookManagerCLI.UI;

public class Menu
{
        private readonly BookService _bookService = new();

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]📚 Book Manager CLI[/]")
                        .PageSize(10)
                        .AddChoices("➕ Add Book", "📜 View Books", "✏️ Update Book", "🔍 Search Books", "❌ Remove Book", "🚪 Exit")
                );

                switch (choice)
                {
                    case "➕ Add Book": AddBook(); break;
                    case "📜 View Books": ViewBooks(); break;
                    case "✏️ Update Book": UpdateBook(); break;  // New option
                    case "🔍 Search Books": SearchBooks(); break;  // New option
                    case "❌ Remove Book": RemoveBook(); break;
                    case "🚪 Exit": return;
                }
            }
        }


    private void AddBook()
    {
        var book = new Book()
        {
            Title = AnsiConsole.Ask<string>("[bold]Enter book title:[/]"),
            Author = AnsiConsole.Ask<string>("[bold]Enter author:[/]"),
            PublishedYear = AnsiConsole.Ask<int>("[bold]Enter published year:[/]"),
            Description = AnsiConsole.Ask<string>("[bold]Enter description:[/]")
        };

        _bookService.AddBook(book);
        AnsiConsole.MarkupLine("[green]✅ Book added successfully![/]");
        AnsiConsole.Prompt(new TextPrompt<string>("Press Enter to continue..."));
    }

    private void ViewBooks()
    {
        var books = _bookService.GetBooks();

        if (books.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]📂 No books available.[/]");
        }
        else
        {
            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("ID")
                .AddColumn("Title")
                .AddColumn("Author")
                .AddColumn("Year")
                .AddColumn("Description");

            foreach (var book in books)
            {
                table.AddRow(book.Id.ToString(), book.Title, book.Author, book.PublishedYear.ToString(), book.Description);
            }

            AnsiConsole.Write(table);
        }

        AnsiConsole.Prompt(new TextPrompt<string>("Press Enter to continue..."));
    }

    private void RemoveBook()
    {
        int id = AnsiConsole.Ask<int>("[bold]Enter Book ID to remove:[/]");

        if (_bookService.RemoveBook(id))
            AnsiConsole.MarkupLine("[green]✅ Book removed successfully![/]");
        else
            AnsiConsole.MarkupLine("[red]❌ Book not found.[/]");

        AnsiConsole.Prompt(new TextPrompt<string>("Press Enter to continue..."));
    }
    
    
    private void SearchBooks()
    {
        string searchTitle = AnsiConsole.Ask<string>("[bold]Enter book title to search:[/]");
        var books = _bookService.SearchBooks(searchTitle);

        if (!books.Any())
        {
            AnsiConsole.MarkupLine("[red]📂 No matching books found.[/]");
        }
        else
        {
            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("ID")
                .AddColumn("Title")
                .AddColumn("Author")
                .AddColumn("Year")
                .AddColumn("Description");

            books.ForEach(book =>
                table.AddRow(book.Id.ToString(), book.Title, book.Author, book.PublishedYear.ToString(), book.Description));

            AnsiConsole.Write(table);
        }

        AnsiConsole.Prompt(new TextPrompt<string>("Press Enter to continue..."));
    }

    private void UpdateBook()
    {
        int id = AnsiConsole.Ask<int>("[bold]Enter Book ID to update:[/]");
        var book = _bookService.GetBookById(id);

        if (book is null)
        {
            AnsiConsole.MarkupLine("[red]❌ Book not found.[/]");
            return;
        }

        string? newTitle = AnsiConsole.Ask<string>("[bold]Enter new title (leave empty to keep unchanged):[/]");
        string? newAuthor = AnsiConsole.Ask<string>("[bold]Enter new author (leave empty to keep unchanged):[/]");
        int? newYear = AnsiConsole.Ask<int?>("[bold]Enter new published year (leave empty to keep unchanged):[/]");
        string? newDescription = AnsiConsole.Ask<string>("[bold]Enter new description (leave empty to keep unchanged):[/]");

        if (_bookService.UpdateBook(id, new BookDto()
            {
                Title = newTitle,
                Author = newAuthor,
                Description = newDescription,
                PublishedYear = newYear
            }))
            AnsiConsole.MarkupLine("[green]✅ Book updated successfully![/]");
        else
            AnsiConsole.MarkupLine("[red]❌ Update failed.[/]");

        AnsiConsole.Prompt(new TextPrompt<string>("Press Enter to continue..."));
    }

}