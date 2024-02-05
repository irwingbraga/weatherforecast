namespace WeatherForecast.UnitTests.Application.UpsertForecast;

using AutoFixture;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
using global::Application.RequestHandlers.CreateForecast;
using global::Application.RequestHandlers.UpsertForecast;

public class UpsertForecastRequestValidatorTests
{
    private readonly UpsertForecastRequestValidator _validator;
    private readonly Fixture _fixture;

    public UpsertForecastRequestValidatorTests()
    {
        _validator = new UpsertForecastRequestValidator();
        _fixture = new Fixture();
    }

    [Theory]
    [InlineAutoData(60)]
    [InlineAutoData(-60)]
    [InlineAutoData(10)]
    [InlineAutoData(-59)]
    public void Should_Pass_When_TemperatureIsWithinRange(int temperature)
    {
        // Arrange
        var request = _fixture.Build<UpsertForecastRequest>()
                              .With(p => p.Temperature, temperature)
                              .With(p => p.Date, DateTime.Today.AddDays(1))
                              .Create();

        // Act & Assert
        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(p => p.Temperature);
    }

    [Theory]
    [InlineAutoData(61)]
    [InlineAutoData(-61)]
    [InlineAutoData(-100)]
    [InlineAutoData(100)]
    public void Should_Fail_When_TemperatureIsOutOfRange(int temperature)
    {
        // Arrange
        var request = _fixture.Build<UpsertForecastRequest>()
                              .With(p => p.Temperature, temperature)
                              .Create();

        // Act & Assert
        _validator.TestValidate(request).ShouldHaveValidationErrorFor(p => p.Temperature);
    }

    [Fact]
    public void Should_Pass_When_DateIsNotInPast()
    {
        // Arrange
        var request = _fixture.Build<UpsertForecastRequest>()
                              .With(p => p.Date, DateTime.Today.AddDays(1))
                              .Create();

        // Act & Assert
        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(p => p.Date);
    }

    [Fact]
    public void Should_Fail_When_DateIsInPast()
    {
        // Arrange
        var request = _fixture.Build<UpsertForecastRequest>()
                              .With(p => p.Date, DateTime.Today.AddDays(-1))
                              .Create();

        // Act & Assert
        _validator.TestValidate(request).ShouldHaveValidationErrorFor(p => p.Date);
    }
}