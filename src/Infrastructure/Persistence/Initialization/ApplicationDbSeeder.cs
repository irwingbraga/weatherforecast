using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Initialization.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Persistence.Initialization;

public class ApplicationDbSeeder(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public async Task SeedDatabaseAsync(CancellationToken cancellationToken)
    {
        await SeedForecastAsync(cancellationToken);
        await SeedTemperatureRangeAsync(cancellationToken);
    }

    private async ValueTask SeedForecastAsync(CancellationToken cancellationToken)
    {
        var dbSet = _db.Set<Forecast>();
        if (!dbSet.Any())
        {
            string data = await ReadDataFileAsync(nameof(Forecast), cancellationToken);
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var yesterday = DateTime.Today.AddDays(-1);
            var dataValues = JsonSerializer.Deserialize<List<ForecastSeedDto>>(data)?
                .Select((value, index) => new Forecast(yesterday.AddDays((index + 1)), value.Temperature))
                .ToList();

            await AddAndSaveData(dbSet, dataValues, cancellationToken);
        }
    }

    private async ValueTask SeedTemperatureRangeAsync(CancellationToken cancellationToken)
    {
        var dbSet = _db.Set<TemperatureRange>();
        if (!dbSet.Any())
        {
            string data = await ReadDataFileAsync(nameof(TemperatureRange), cancellationToken);
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var dataValues = JsonSerializer.Deserialize<List<TemperatureRangeSeedDto>>(data)?
                .Select(value => new TemperatureRange(value.MinTemperature, value.MaxTemperature, value.Description))
                .ToList();

            await AddAndSaveData(dbSet, dataValues, cancellationToken);
        }
    }

    private async Task AddAndSaveData<T>(DbSet<T> dbSet, List<T>? dataValues, CancellationToken cancellationToken)
        where T : class
    {
        if (dataValues != null)
        {
            foreach (var dataValue in dataValues)
            {
                await dbSet.AddAsync(dataValue, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static async ValueTask<string> ReadDataFileAsync(string entityName, CancellationToken cancellationToken)
    {
        try
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (path == null)
            {
                return string.Empty;
            }

            return await File.ReadAllTextAsync(path + $"/Persistence/Initialization/Data/{entityName.ToLower()}.json", cancellationToken);
        }
        catch (Exception err)
        {
            Debug.WriteLine(err);
            return string.Empty;
        }
    }
}
