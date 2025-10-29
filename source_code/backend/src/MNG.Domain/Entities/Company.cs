using MNG.Domain.Abstractions;

namespace MNG.Domain.Entities;
public class Company: EntityBase<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Addess { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Web { get; set; }
    public bool IsActive { get; set; }
}
