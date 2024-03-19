using MediatR;

namespace CustomerService.Domain.SeedWork
{
    public interface IObjectNotification : IRequest<object?>;
    public interface IObjectNotificationHandler<T> : IRequestHandler<T, object?>
        where T : IObjectNotification;
}
