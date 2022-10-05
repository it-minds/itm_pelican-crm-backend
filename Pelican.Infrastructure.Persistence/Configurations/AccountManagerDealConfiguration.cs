using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class AccountManagerDealConfiguration : IEntityTypeConfiguration<AccountManagerDeal>
{
	public void Configure(EntityTypeBuilder<AccountManagerDeal> builder)
	{
		builder.Property(p => p.IsActive)
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
