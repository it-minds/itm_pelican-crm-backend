using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Configurations;
public class EmailConfiguration : IEntityTypeConfiguration<Email>
{
	public void Configure(EntityTypeBuilder<Email> builder)
	{
		builder.ToTable("Emails");

		builder.Property(e => e.Name)
		  .HasMaxLength(StringLengths.Name)
		  .IsRequired();

		builder.Property(e => e.Subject)
		  .HasMaxLength(StringLengths.SubjectLine)
		  .IsRequired();

		builder.Property(e => e.Heading1)
			.HasMaxLength(StringLengths.Heading)
			.IsRequired();

		builder.Property(e => e.Paragraph1)
			.HasMaxLength(StringLengths.Paragraph)
			.IsRequired();

		builder.Property(e => e.Heading2)
			.HasMaxLength(StringLengths.Heading)
			.IsRequired();

		builder.Property(e => e.Paragraph2)
			.HasMaxLength(StringLengths.Paragraph)
			.IsRequired();

		builder.Property(e => e.Heading3)
			.HasMaxLength(StringLengths.Heading)
			.IsRequired();

		builder.Property(e => e.Paragraph3)
			.HasMaxLength(StringLengths.Paragraph)
			.IsRequired();

		builder.Property(e => e.CtaButtonText)
		  .HasMaxLength(StringLengths.CtaButtonText)
		  .IsRequired();

		builder.Property(e => e.EmailType)
			.IsRequired();

		builder.HasIndex(e => e.EmailType)
			.IsUnique();
	}
}
