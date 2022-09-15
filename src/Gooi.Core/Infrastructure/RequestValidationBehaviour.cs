using MediatR;
using FluentValidation;

namespace Gooi.Core.Infrastructure;
public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
  {
    _validators = validators;
  }

  public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
  {
    var context = new ValidationContext<TRequest>(request);

    var validationResult = _validators.Select(v => v.Validate(context)).ToList();

    if (validationResult.Any(x => !x.IsValid))
    {
      var failureAggregate = validationResult.SelectMany(s => s.Errors).Where(x => x != null).ToList();
      throw new ValidationException(failureAggregate);
    }

    return next();
  }
}
