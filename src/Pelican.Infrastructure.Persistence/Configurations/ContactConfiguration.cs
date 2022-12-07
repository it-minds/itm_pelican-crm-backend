using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
	public void Configure(EntityTypeBuilder<Contact> builder)
	{
		builder.ToTable("Contacts");

		builder.Property(p => p.FirstName)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.LastName)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.Email)
			.HasMaxLength(StringLengths.Email);

		builder.Property(p => p.PhoneNumber)
			.HasMaxLength(StringLengths.PhoneNumber);

		builder.Property(p => p.SourceId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.SourceOwnerId)
			.HasMaxLength(StringLengths.Id);

		builder.Property(p => p.JobTitle)
			.HasMaxLength(StringLengths.JobTitle);

		builder.Property(p => p.Source)
			.HasMaxLength(StringLengths.Source)
			.IsRequired();

		builder.HasMany(a => a.ClientContacts)
			.WithOne(e => e.Contact)
			.HasForeignKey(a => a.ContactId)
			.IsRequired();

		builder.HasMany(a => a.DealContacts)
			.WithOne(a => a.Contact)
			.HasForeignKey(a => a.ContactId)
			.IsRequired();
	}
}
