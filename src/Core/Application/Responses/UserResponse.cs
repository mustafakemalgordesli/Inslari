namespace Application.Responses;

public class UserResponse
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public string? ImageUrl { get; set; }
    public string Culture { get; set; } = "en";
}
