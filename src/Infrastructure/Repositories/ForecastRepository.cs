using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ForecastRepository(ApplicationDbContext dbContext) : IForecastRepository
{
    private readonly DbSet<Forecast> _dbSet = dbContext.Set<Forecast>();

    public Task<Forecast?> GetByDateAsync(DateTime dateTime, CancellationToken cancellationToken)
    {
        return _dbSet.FirstOrDefaultAsync(p => p.Date.Date == dateTime.Date, cancellationToken);
    }

    public async Task<IEnumerable<Forecast>> GetByRangeDateAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return await _dbSet.Where(p => p.Date.Date >= startDate.Date && p.Date.Date <= endDate.Date).OrderBy(p => p.Date).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Forecast forecast, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(forecast, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Forecast forecast, CancellationToken cancellationToken)
    {
        _dbSet.Update(forecast);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
