using Domain.Common;

namespace Domain.Entities;

public class Contact : BaseEntity
{
    public Guid UserId { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? Message { get; set; }
}
