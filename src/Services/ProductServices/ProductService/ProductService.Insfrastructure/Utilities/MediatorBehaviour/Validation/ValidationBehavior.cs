using FluentValidation;
using MediatR;
using ProductService.Insfrastructure.Utilities.MediatorBehaviour.Validation.Exceptions;

namespace ProductService.Insfrastructure.Utilities.MediatorBehaviour.Validation
{
    /// <summary>
    /// Validation pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

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
}