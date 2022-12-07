using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class AccountManagerDealConfiguration : IEntityTypeConfiguration<AccountManagerDeal>
{
	public void Configure(EntityTypeBuilder<AccountManagerDeal> builder)
	{
		builder.ToTable("AccountManagerDeals");

		builder.Property(p => p.IsActive)
			.IsRequired();

		builder.Property(p => p.SourceAccountManagerId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.SourceDealId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.HasOne(a => a.AccountManager)
			.WithMany(e => e.AccountManagerDeals)
			.HasForeignKey(a => a.AccountManagerId)
			.IsRequired();

		builder.HasOne(a => a.Deal)
			.WithMany(a => a.AccountManagerDeals)
			.HasForeignKey(a => a.DealId)
			.IsRequired();
	}
}
