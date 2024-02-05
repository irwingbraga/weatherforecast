namespace WeatherForecast.UnitTests.Application.UpsertForecast;
using AutoFixture;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using global::Application.RequestHandlers.UpsertForecast;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class UpsertForecastRequestHandlerTests
{
    private readonly IFixture _fixture;
    private readonly IForecastRepository _repository;
    private readonly UpsertForecastRequestHandler _handler;

    public UpsertForecastRequestHandlerTests()
    {
        _fixture = new Fixture();
        _repository = Substitute.For<IForecastRepository>();
        _handler = new UpsertForecastRequestHandler(_repository);
    }

    [Fact]
    public async Task Handle_NewForecast_ShouldAddForecast()
    {
        // Arrange
        var request = _fixture.Build<UpsertForecastRequest>()
            .With(f => f.Temperature, 25)
            .Create();
        _repository.GetByDateAsync(request.Date, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Forecast>(null));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _repository.Received(1).AddAsync(Arg.Any<Forecast>(), Arg.Any<CancellationToken>());
        result.Should().NotBe(Guid.Empty, because: "a new Forecast should have been created with a non-empty GUID as its ID.");
    }

    [Fact]
    public async Task Handle_ExistingForecast_ShouldUpdateForecast()
    {
        // Arrange
        var request = _fixture.Build<UpsertForecastRequest>()
            .With(f => f.Temperature, 25)
            .Create();
        var existingForecast = new Forecast(request.Date, 30);

        _repository.GetByDateAsync(request.Date, Arg.Any<CancellationToken>()).Returns(Task.FromResult(existingForecast));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _repository.Received(1).UpdateAsync(Arg.Any<Forecast>(), Arg.Any<CancellationToken>());
        result.Should().Be(existingForecast.Id, because: "the existing Forecast should be updated and its ID returned.");
    }
}

