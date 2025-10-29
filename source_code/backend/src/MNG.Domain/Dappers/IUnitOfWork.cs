using MNG.Domain.Dappers.Repositories.Product;

namespace MNG.Domain.Dappers;
public interface IUnitOfWork
{
    IProductRepository Products { get; }
}
