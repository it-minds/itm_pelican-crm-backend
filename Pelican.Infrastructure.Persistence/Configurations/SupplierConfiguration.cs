﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
	public void Configure(EntityTypeBuilder<Supplier> builder)
	{
		builder.Property(p => p.Name)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.PictureUrl)
			.HasMaxLength(StringLengths.Url);

		builder.Property(p => p.PhoneNumber)
			.HasMaxLength(StringLengths.PhoneNumber);

		builder.Property(p => p.Email)
			.HasMaxLength(StringLengths.Email);

		builder.Property(p => p.LinkedInUrl)
			.HasMaxLength(StringLengths.Url);

		builder.Property(p => p.WebsiteUrl)
			.HasMaxLength(StringLengths.Url);

		builder.HasMany(a => a.AccountManagers)
			.WithOne(a => a.Supplier)
			.HasForeignKey(a => a.SupplierId);

		builder.HasMany(a => a.OfficeLocations)
			.WithOne(a => a.Supplier)
			.HasForeignKey(a => a.SupplierId);
	}
}
