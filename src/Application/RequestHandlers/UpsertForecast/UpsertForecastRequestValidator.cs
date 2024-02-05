using Application.RequestHandlers.UpsertForecast;
using FluentValidation;

namespace Application.RequestHandlers.CreateForecast;

public class UpsertForecastRequestValidator : AbstractValidator<UpsertForecastRequest>
{
	public UpsertForecastRequestValidator()
	{
		RuleFor(p => p.Temperature)
			.InclusiveBetween(-60, 60)
			.WithMessage("Temperature must be between -60 and +60 degrees.");

		RuleFor(p => p.Date)
			.GreaterThanOrEqualTo(DateTime.Today.Date)
			.WithMessage("Input forecasts cannot be in the past");
	}
}