namespace Application.Responses;

public class ContactResponse
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
