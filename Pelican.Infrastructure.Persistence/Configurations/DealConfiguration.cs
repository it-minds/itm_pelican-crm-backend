﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class DealConfiguration : IEntityTypeConfiguration<Deal>
{
	public void Configure(EntityTypeBuilder<Deal> builder)
	{
		builder.Property(p => p.DealStatus)
			.HasMaxLength(StringLengths.DealStatus)
			.IsRequired();

		builder.Property(p => p.EndDate)
			.IsRequired();

		builder.HasMany(a => a.AccountManagerDeals)
			.WithOne(e => e.Deal)
			.HasForeignKey(a => a.DealId)
			.IsRequired();

		builder.HasOne(a => a.Client)
			.WithMany(a => a.Deals)
			.HasForeignKey(a => a.ClientId);

		builder.HasMany(a => a.DealContacts)
			.WithOne(a => a.Deal)
			.HasForeignKey(a => a.DealId);

	}
}
