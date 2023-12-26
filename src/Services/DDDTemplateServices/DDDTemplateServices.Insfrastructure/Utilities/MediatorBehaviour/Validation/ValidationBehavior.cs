using DDDTemplateServices.Insfrastructure.Utilities.MediatorBehaviour.Validation.Exceptions;
using FluentValidation;
using MediatR;
namespace DDDTemplateServices.Insfrastructure.Utilities.MediatorBehaviour.Validation;
/// <summary>
/// Validation pipeline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        //Fluent Validation Remove Casstle Autofac
        //this.validators = validators.Where(x => x.GetType().FullName != "Castle.Proxies.IValidator`1Proxy");
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next.Invoke();
        }
        var context = new ValidationContext<TRequest>(request);
        var errors = _validators.Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList()
            .Select(x => new ValidationData(x.PropertyName, x.ErrorMessage));
        if (errors.Any())
        {
            throw new CustomValidatonException(errors);
        }
        return await next.Invoke();
    }
}