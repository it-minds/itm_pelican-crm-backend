﻿using Microsoft.EntityFrameworkCore;
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

		builder.Property(p => p.Segment)
			.HasMaxLength(StringLengths.Industry);

		builder.Property(p => p.Website)
			.HasMaxLength(StringLengths.Url);

		builder.Property(p => p.Classification)
			.HasMaxLength(StringLengths.Classification);

		builder.Property(p => p.HubSpotId)
			.HasMaxLength(StringLengths.Id)
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
