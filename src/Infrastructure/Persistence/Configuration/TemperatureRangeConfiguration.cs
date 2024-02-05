using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
	public class TemperatureRangeConfiguration : IEntityTypeConfiguration<TemperatureRange>
	{
		public void Configure(EntityTypeBuilder<TemperatureRange> builder)
		{
			builder.ToTable("TemperatureRanges");

			builder.HasKey(tr => tr.Id);

			builder.Property(tr => tr.Id)
				.IsRequired();

			builder.OwnsOne(tr => tr.MinTemperature, minTemp =>
			{
				minTemp.Property(t => t.Value).HasColumnName("MinTemperature");
			});

			builder.OwnsOne(tr => tr.MaxTemperature, maxTemp =>
			{
				maxTemp.Property(t => t.Value).HasColumnName("MaxTemperature");
			});

			builder.Property(tr => tr.Description)
				.IsRequired()
				.HasMaxLength(255);
		}
	}
}
