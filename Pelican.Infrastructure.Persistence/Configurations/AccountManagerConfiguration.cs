using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class AccountManagerConfiguration : IEntityTypeConfiguration<AccountManager>
{
	public void Configure(EntityTypeBuilder<AccountManager> builder)
	{
		builder.ToTable("AccountManagers");

		builder.Property(p => p.FirstName)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.LastName)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.PictureUrl)
			.HasMaxLength(StringLengths.Url);

		builder.Property(p => p.Email)
			.HasMaxLength(StringLengths.Email)
			.IsRequired();

		builder.Property(p => p.LinkedInUrl)
			.HasMaxLength(StringLengths.Url);

		builder.Property(p => p.PhoneNumber)
			.HasMaxLength(StringLengths.PhoneNumber);

		builder.Property(p => p.HubSpotId)
			.HasMaxLength(StringLengths.Id);

		builder.HasOne(a => a.Supplier)
			.WithMany(e => e.AccountManagers)
			.HasForeignKey(a => a.SupplierId)
			.IsRequired();

		builder.HasMany(a => a.AccountManagerDeals)
			.WithOne(a => a.AccountManager)
			.HasForeignKey(a => a.AccountManagerId)
			.IsRequired();
	}
}
