using Domain.Common;

namespace Domain.Entities;

public class Language : BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}
