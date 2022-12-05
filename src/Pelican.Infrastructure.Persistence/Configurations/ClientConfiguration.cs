using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
	public void Configure(EntityTypeBuilder<Client> builder)
	{
		builder.ToTable("Clients");

		builder.Property(p => p.Name)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.PictureUrl)
			.HasMaxLength(StringLengths.Url);

		builder.Property(p => p.OfficeLocation)
			.HasMaxLength(StringLengths.OfficeLocation);

		builder.Property(p => p.Website)
			.HasMaxLength(StringLengths.Url);

		builder.Property(p => p.SourceId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.Source)
			.HasMaxLength(StringLengths.Source)
			.IsRequired();

		builder.HasMany(a => a.ClientContacts)
			.WithOne(a => a.Client)
			.HasForeignKey(a => a.ClientId);

		builder.HasMany(a => a.Deals)
			.WithOne(a => a.Client)
			.HasForeignKey(a => a.ClientId);
	}
}
