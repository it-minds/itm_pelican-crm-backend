using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
	public void Configure(EntityTypeBuilder<Client> builder)
	{
		builder.Property(p => p.Name)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.PictureUrl)
			.HasMaxLength(StringLengths.Url)
			.IsRequired();

		builder.Property(p => p.OfficeLocation)
			.HasMaxLength(StringLengths.OfficeLocation)
			.IsRequired();

		builder.Property(p => p.Segment)
			.HasMaxLength(StringLengths.Industry)
			.IsRequired();

		builder.Property(p => p.Classification)
			.HasMaxLength(StringLengths.Classification)
			.IsRequired();

		builder.HasMany(a => a.ClientContacts)
			.WithOne(a => a.Client)
			.HasForeignKey(a => a.ClientId)
			.IsRequired();

		builder.HasMany(a => a.Deals)
			.WithOne(a => a.Client)
			.HasForeignKey(a => a.ClientId)
			.IsRequired();
	}
}
