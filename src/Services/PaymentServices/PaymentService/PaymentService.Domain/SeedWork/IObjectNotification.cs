using MediatR;

namespace PaymentService.Domain.SeedWork
{
    public interface IObjectNotification : IRequest<object?>;
    public interface IObjectNotificationHandler<T> : IRequestHandler<T, object?>
        where T : IObjectNotification;
}
