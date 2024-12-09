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
    public bool IsActive { get; set; }

    public User()
    {
        UserRegistered();
    }

    public void UserRegistered()
    {
        RaiseDomainEvent(new UserRegisteredDomainEvent(this.Id, this.Email));
    }
}
