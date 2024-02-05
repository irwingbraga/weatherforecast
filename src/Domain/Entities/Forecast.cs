using Domain.ValueObjects;

namespace Domain.Entities;

public class Forecast
{
    private Forecast() { }
    public Guid Id { get; set; }
	public DateTime Date { get; private set; }
	public Temperature Temperature { get; private set; }

	public Forecast(DateTime date, int temperature)
	{
		Id = Guid.NewGuid();
		Date = date;
		UpdateTemperature(temperature);
	}

	public void UpdateTemperature(int temperature)
	{
		Temperature = new Temperature(temperature);
	}
}
