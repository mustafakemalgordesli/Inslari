using Domain.Common;

namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public required string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public virtual User User { get; set; }
}
