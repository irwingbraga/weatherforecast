using Domain.Entities;

namespace Domain.Repositories;

public interface ITemperatureRangeRepository
{
    Task<IEnumerable<TemperatureRange>> GetAllAsync(CancellationToken cancellationToken);
}
