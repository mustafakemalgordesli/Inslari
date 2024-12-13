using Domain.Common;
using Domain.DomainEvents;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? ImageUrl { get; set; }
    public string Culture { get; set; } = "en";
    public bool IsActive { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];


    public void UserRegistered()
    {
        RaiseDomainEvent(new UserRegisteredDomainEvent(this.Id, this.Email));
    }
}
