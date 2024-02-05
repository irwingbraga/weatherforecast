namespace WeatherForecast.UnitTests.Application.GetForecasts;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using global::Application.RequestHandlers.GetForecasts;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class GetForecastsRequestHandlerTests
{
    private readonly IFixture _fixture;
    private readonly IForecastRepository _forecastRepository;
    private readonly ITemperatureRangeRepository _temperatureRangeRepository;
    private readonly GetForecastsRequestHandler _handler;

    public GetForecastsRequestHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _forecastRepository = _fixture.Freeze<IForecastRepository>();
        _temperatureRangeRepository = _fixture.Freeze<ITemperatureRangeRepository>();
        _handler = new GetForecastsRequestHandler(_forecastRepository, _temperatureRangeRepository);
    }

    [Fact]
    public async Task Handle_WhenForecastsAreAvailable_ShouldReturnForecasts()
    {
        // Arrange
        var request = _fixture.Create<GetForecastsRequest>();

        _fixture.Customizations.Add(
                    new RandomNumericSequenceGenerator(-60, 61));

        var forecasts = _fixture.CreateMany<Forecast>(5);

        _forecastRepository.GetByRangeDateAsync(request.StartDate, request.StartDate.AddDays(7), Arg.Any<CancellationToken>())
            .Returns(forecasts);

        _fixture.Customize<TemperatureRange>(composer => composer.FromFactory(() =>
        {
            var minTemperatureValue = _fixture.Create<int>() % 121 - 60;

            if (minTemperatureValue < -60)
            {
                minTemperatureValue = -60;
            }

            var maxTemperatureValue = minTemperatureValue + _fixture.Create<int>() % (61 - minTemperatureValue) + 1;

            if (minTemperatureValue >= maxTemperatureValue)
            {
                maxTemperatureValue = minTemperatureValue + 1;
            }

            return new TemperatureRange(minTemperatureValue, maxTemperatureValue, "Valid range");
        }));

        var temperatureRanges = _fixture.CreateMany<TemperatureRange>(3).ToList();
        _temperatureRangeRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(temperatureRanges);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(forecasts.Count());
    }

    [Fact]
    public async Task Handle_WhenNoForecastsAvailable_ShouldReturnNull()
    {
        // Arrange
        var request = _fixture.Create<GetForecastsRequest>();
        _forecastRepository.GetByRangeDateAsync(request.StartDate, request.StartDate.AddDays(7), Arg.Any<CancellationToken>())
            .Returns(Enumerable.Empty<Forecast>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
