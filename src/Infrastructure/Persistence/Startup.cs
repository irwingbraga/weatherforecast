using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Initialization;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class Startup
{
	public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration config)
	{
		var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
		string? connectionString = databaseSettings?.ConnectionString;
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new InvalidOperationException("DB ConnectionString is not configured.");
		}

		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		return services
			.Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))

			.AddDbContext<ApplicationDbContext>(builder => builder.UseNpgsql(connectionString, e =>
					 e.MigrationsAssembly("Migrators.PostgreSQL")))

			.AddTransient<IDatabaseInitializer, ApplicationDbInitializer>()
			.AddTransient<ApplicationDbInitializer>()
			.AddTransient<ApplicationDbSeeder>()
			.AddRepositories()
			.AddHostedService<DatabaseInitializerBackgroundService>();
	}

	private static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		return services
				.AddScoped<IForecastRepository, ForecastRepository>()
				.AddScoped<ITemperatureRangeRepository, TemperatureRangeRepository>();
	}
}
