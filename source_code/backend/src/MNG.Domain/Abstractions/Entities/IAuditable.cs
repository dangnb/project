namespace MNG.Domain.Abstractions.Entities;
public interface IAuditable : IDateTracking, IUserTracking, ISoftDelete
{
}
