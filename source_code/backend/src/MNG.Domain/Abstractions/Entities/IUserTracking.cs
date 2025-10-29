namespace MNG.Domain.Abstractions.Entities;
public interface IUserTracking
{
    string CreatedBy { get; set; }
    string? LastModifiedBy { get; set; }
}
