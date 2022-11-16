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

		builder.Property(p => p.Firstname)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.Lastname)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(p => p.Email)
			.HasMaxLength(StringLengths.Email);

		builder.Property(p => p.LinkedInUrl)
			.HasMaxLength(StringLengths.Url);

		builder.Property(p => p.PhoneNumber)
			.HasMaxLength(StringLengths.PhoneNumber);

		builder.Property(p => p.HubSpotId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.HubSpotOwnerId)
			.HasMaxLength(StringLengths.Id);

		builder.Property(p => p.JobTitle)
			.HasMaxLength(StringLengths.JobTitle);

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
