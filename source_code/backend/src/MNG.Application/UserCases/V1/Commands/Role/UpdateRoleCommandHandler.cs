
//using AutoMapper;
//using MNG.Contract.Abstractions.Message;
//using MNG.Contract.Abstractions.Shared;
//using MNG.Contract.Services.V1.Product;
//using MNG.Domain.Abstractions;
//using MNG.Domain.Abstractions.Repositories;
//using MNG.Domain.Entities;
//using MediatR;

//namespace MNG.Application.UserCases.V1.Commands.Product;
//public sealed class UpdateProductCommandHandler : ICommandHandler<Command.UpdateProductCommand>
//{
//    private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepositoryBase;
//    private readonly IMapper _mapper;
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IPublisher _publisher;


//    public UpdateProductCommandHandler(
//        IRepositoryBase<Domain.Entities.Product, Guid> productRepositoryBase,
//        IPublisher publisher,
//        IMapper mapper,
//        IUnitOfWork unitOfWork)
//    {
//        _productRepositoryBase = productRepositoryBase;
//        _publisher = publisher;
//        _mapper = mapper;
//        _unitOfWork = unitOfWork;
//    }
//    public async Task<Result> Handle(Command.UpdateProductCommand request, CancellationToken cancellationToken)
//    {
//        var product = await _productRepositoryBase.FindByIdAsync(request.Id);
//        product.Update(request.Name, request.Price, request.Description);
//        _productRepositoryBase.Update(product);
//        await _unitOfWork.SaveChangesAsync(cancellationToken);
//        await _publisher.Publish(new DomainEvent.ProductUpdated(Guid.NewGuid()));
//        return Result.Success(product);
//    }
//}
