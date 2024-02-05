using Domain.ValueObjects;

namespace Domain.Entities;

public class TemperatureRange
{
	private TemperatureRange() { }
	public Guid Id { get; set; }
	public Temperature MinTemperature { get; private set; }
	public Temperature MaxTemperature { get; private set; }
	public string Description { get; private set; }

	public TemperatureRange(int minTemperature, int maxTemperature, string description)
	{
		if (minTemperature >= maxTemperature)
			throw new ArgumentException("MinTemperature must be less than MaxTemperature.");

		Id = Guid.NewGuid();
		MinTemperature = new Temperature(minTemperature);
		MaxTemperature = new Temperature(maxTemperature);
		Description = description ?? throw new ArgumentNullException(nameof(description));
	}

	public bool Includes(int temperature)
	{
		return temperature >= MinTemperature.Value && temperature <= MaxTemperature.Value;
	}
}
