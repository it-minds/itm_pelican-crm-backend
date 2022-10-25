﻿namespace Pelican.Infrastructure.Persistence.Configurations;
internal class ClientContactConfiguration : IEntityTypeConfiguration<ClientContact>
{
	public void Configure(EntityTypeBuilder<ClientContact> builder)
	{
		builder.ToTable("ClientContacts");

		builder.Property(p => p.IsActive)
			.IsRequired();

		builder.Property(p => p.HubSpotClientId)
			.HasMaxLength(StringLengths.Id)
			.IsRequired();

		builder.Property(p => p.HubSpotContactId)
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
