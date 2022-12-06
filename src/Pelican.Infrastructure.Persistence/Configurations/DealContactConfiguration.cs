using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class DealContactConfiguration : IEntityTypeConfiguration<DealContact>
{
	public void Configure(EntityTypeBuilder<DealContact> builder)
	{
		builder.ToTable("DealContacts");

		builder.Property(p => p.IsActive)
			.IsRequired();

		builder.Property(p => p.SourceContactId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.SourceDealId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.Source)
			.HasMaxLength(StringLengths.Source)
			.IsRequired();

		builder.HasOne(a => a.Contact)
			.WithMany(e => e.DealContacts)
			.HasForeignKey(a => a.ContactId)
			.IsRequired();

		builder.HasOne(a => a.Deal)
			.WithMany(a => a.DealContacts)
			.HasForeignKey(a => a.DealId)
			.IsRequired();
	}
}
