using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class LocationConfiguration : IEntityTypeConfiguration<Location>
{
	public void Configure(EntityTypeBuilder<Location> builder)
	{
		builder.ToTable("Locations");

		builder.Property(p => p.CityName)
			.HasMaxLength(StringLengths.OfficeLocation)
			.IsRequired();

		builder.HasOne(a => a.Supplier)
			.WithMany(a => a.OfficeLocations)
			.HasForeignKey(a => a.SupplierId);
	}
}
