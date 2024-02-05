namespace Domain.ValueObjects;

public class Temperature
{
	public int Value { get; private set; }

	public Temperature(int value)
	{
		if (value < -60 || value > 60)
			throw new ArgumentOutOfRangeException(nameof(value), "Temperature must be between -60 and +60 degrees.");

		Value = value;
	}
}
