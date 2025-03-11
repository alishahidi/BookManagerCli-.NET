namespace BookManagerCLI.Dto;

public class BookDto
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? PublishedYear { get; set; }
    public string? Description { get; set; }
}