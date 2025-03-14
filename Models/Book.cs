using System.ComponentModel.DataAnnotations;

namespace BookManagerCLI.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Author { get; set; }
    public int PublishedYear { get; set; }
    public string Description { get; set; }
}