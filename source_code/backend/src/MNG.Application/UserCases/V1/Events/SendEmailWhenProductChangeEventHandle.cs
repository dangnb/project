using MNG.Contract.Abstractions.Message;
using MNG.Contract.Services.V1.Product;

namespace MNG.Application.UserCases.V1.Events;
public class SendEmailWhenProductChangeEventHandle : IDomainEventHandler<DomainEvent.ProductCreated>
    , IDomainEventHandler<DomainEvent.ProductDeleted>
    , IDomainEventHandler<DomainEvent.ProductUpdated>
{
    public async Task Handle(DomainEvent.ProductCreated notification, CancellationToken cancellationToken)
    {
        await Task.Delay(1000);
    }

    public Task Handle(DomainEvent.ProductDeleted notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Handle(DomainEvent.ProductUpdated notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
