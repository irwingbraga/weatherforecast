using FluentValidation;
using MediatR;

namespace Application.Behaviors
{
	internal class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

		public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (_validators?.Any() != true)
			{
				return next();
			}

			var context = new ValidationContext<TRequest>(request);
			var errors = _validators
				.Select(x => x.Validate(context))
				.SelectMany(x => x.Errors)
				.Where(x => x != null)
				.ToList();

			if (errors?.Count > 0)
			{
				throw new ValidationException(errors);
			}

			return next();
		}
	}
}
