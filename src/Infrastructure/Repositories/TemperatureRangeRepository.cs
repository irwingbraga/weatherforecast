using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class TemperatureRangeRepository(ApplicationDbContext dbContext) : ITemperatureRangeRepository
{
    private readonly DbSet<TemperatureRange> _dbSet = dbContext.Set<TemperatureRange>();

    public async Task<IEnumerable<TemperatureRange>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }
}
