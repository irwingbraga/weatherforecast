using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
	public class ForecastConfiguration : IEntityTypeConfiguration<Forecast>
	{
		public void Configure(EntityTypeBuilder<Forecast> builder)
		{
			builder.ToTable("Forecasts");

			builder.HasKey(tr => tr.Id);

			builder.Property(tr => tr.Id)
				.IsRequired();

			builder.OwnsOne(tr => tr.Temperature, temp =>
			{
				temp.Property(t => t.Value).HasColumnName("Temperature");
			});

			builder.Property(tr => tr.Date)
				.IsRequired();
		}
	}
}
