using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class DealConfiguration : IEntityTypeConfiguration<Deal>
{
	public void Configure(EntityTypeBuilder<Deal> builder)
	{
		builder.ToTable("Deals");

		builder.Property(p => p.Status)
			.HasMaxLength(StringLengths.DealStatus);

		builder.Property(p => p.SourceId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.SourceOwnerId)
			.HasMaxLength(StringLengths.Id);

		builder.Property(p => p.Description)
			.HasMaxLength(StringLengths.DealDescription);

		builder.Property(p => p.Name)
			.HasMaxLength(StringLengths.DealName);

		builder.Property(p => p.Source)
			.HasMaxLength(StringLengths.Source)
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
