using MNG.Contract.Abstractions.Message;
using MNG.Contract.Services.V1.Product;
using MediatR;

namespace MNG.Application.UserCases.V1.Events;
public class SendSmslWhenProductChangeEventHandle :
    IDomainEventHandler<DomainEvent.ProductCreated>,
    IDomainEventHandler<DomainEvent.ProductUpdated>,
    IDomainEventHandler<DomainEvent.ProductDeleted>
{
    public Task Handle(DomainEvent.ProductDeleted notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task INotificationHandler<DomainEvent.ProductCreated>.Handle(DomainEvent.ProductCreated notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task INotificationHandler<DomainEvent.ProductUpdated>.Handle(DomainEvent.ProductUpdated notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
