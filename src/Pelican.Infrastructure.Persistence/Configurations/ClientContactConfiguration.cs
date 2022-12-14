using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
internal class ClientContactConfiguration : IEntityTypeConfiguration<ClientContact>
{
	public void Configure(EntityTypeBuilder<ClientContact> builder)
	{
		builder.ToTable("ClientContacts");

		builder.Property(p => p.IsActive)
			.IsRequired();

		builder.Property(p => p.SourceClientId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.SourceContactId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.HasOne(a => a.Contact)
			.WithMany(e => e.ClientContacts)
			.HasForeignKey(a => a.ContactId)
			.IsRequired();

		builder.HasOne(a => a.Client)
			.WithMany(a => a.ClientContacts)
			.HasForeignKey(a => a.ClientId)
			.IsRequired();
	}
}
