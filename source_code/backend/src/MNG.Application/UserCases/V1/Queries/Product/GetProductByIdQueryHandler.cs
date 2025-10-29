using AutoMapper;
using MNG.Contract.Abstractions.Message;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Services.V1.Product;
using MNG.Domain.Abstractions;
using MNG.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MNG.Application.UserCases.V1.Queries.Product;
public class GetProductByIdQueryHandler : IQueryHandler<Query.GetPrductByIdQuery, Response.ProductResponse>
{
    private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepositoryBase;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepositoryBase, IMapper mapper)
    {
        _productRepositoryBase = productRepositoryBase;
        _mapper = mapper;
    }
    public async Task<Result<Response.ProductResponse>> Handle(Query.GetPrductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepositoryBase.FindByIdAsync(request.Id);
        var result = _mapper.Map<Response.ProductResponse>(product);
        return result;
    }
}
