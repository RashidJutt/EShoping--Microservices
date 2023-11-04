using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviours;

public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) { return await next(); }

        var validationResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var errors = validationResult.SelectMany(v => v.Errors);
        if (errors.Any())
        {
            throw new Exceptions.ValidationException(errors);
        }

        return await next();
    }
}
