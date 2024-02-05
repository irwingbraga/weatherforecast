using Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace Infrastructure.Persistence;
internal class DatabaseInitializerBackgroundService : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;

	public DatabaseInitializerBackgroundService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var scope = _serviceProvider.CreateScope();
		var retryPolicy = Policy
			.Handle<Exception>()
			.WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

		await retryPolicy.ExecuteAsync(async () =>
		{
			var migrationService = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
			await migrationService.InitializeDatabasesAsync(stoppingToken);
		});
	}
}
