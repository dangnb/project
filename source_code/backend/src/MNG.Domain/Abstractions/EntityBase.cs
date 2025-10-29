using MNG.Domain.Abstractions.Entities;

namespace MNG.Domain.Abstractions;
public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}
