
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Initialization;

internal class ApplicationDbInitializer : IDatabaseInitializer
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ApplicationDbSeeder _dbSeeder;

	public ApplicationDbInitializer(ApplicationDbContext dbContext, ApplicationDbSeeder dbSeeder)
	{
		_dbContext = dbContext;
		_dbSeeder = dbSeeder;
	}

	public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
	{
		if (_dbContext.Database.GetMigrations().Any())
		{
			if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
			{
				await _dbContext.Database.MigrateAsync(cancellationToken);
			}

			if (await _dbContext.Database.CanConnectAsync(cancellationToken))
			{
				await _dbSeeder.SeedDatabaseAsync(cancellationToken);
			}
		}
	}
}
