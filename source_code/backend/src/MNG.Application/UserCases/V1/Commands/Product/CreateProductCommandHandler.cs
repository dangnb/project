
using AutoMapper;
using MNG.Contract.Abstractions.Message;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Services.V1.Product;
using MNG.Domain.Abstractions;
using MNG.Domain.Abstractions.Repositories;
using MediatR;
using MNG.Persistence;

namespace MNG.Application.UserCases.V1.Commands.Product;
public sealed class CreateProductCommandHandler : ICommandHandler<Command.CreateProductCommand>
{
    private readonly IRepositoryBaseDBContext<ApplicationDbContext,Domain.Entities.Product, Guid> _productRepositoryBase;
    private readonly IMapper _mapper;
    private readonly IUnitOfWorkDbContext<ApplicationDbContext> _unitOfWork;
    private readonly IPublisher _publisher;


    public CreateProductCommandHandler(IPublisher publisher, IRepositoryBaseDBContext<ApplicationDbContext, Domain.Entities.Product, Guid> productRepositoryBase, IMapper mapper, IUnitOfWorkDbContext<ApplicationDbContext> unitOfWork)
    {
        _productRepositoryBase = productRepositoryBase;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    public async Task<Result> Handle(Command.CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Domain.Entities.Product.CreateProduct(Guid.NewGuid(),request.Name, request.Price, request.Description);
        _productRepositoryBase.Add(product);
        //await _unitOfWork.SaveChangesAsync(cancellationToken);

        //await _publisher.Publish(new DomainEvent.ProductCreated(Guid.NewGuid()));

        return Result.Success(product);
    }
}
