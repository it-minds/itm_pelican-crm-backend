using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Enums;

namespace Pelican.Infrastructure.Persistence.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");

		builder.HasDiscriminator<RoleEnum>("Role")
			.HasValue<AdminUser>(RoleEnum.Admin)
			.HasValue<StandardUser>(RoleEnum.Standard);

		builder.Property(e => e.Name)
			.HasMaxLength(StringLengths.Name)
			.IsRequired();

		builder.Property(e => e.Email)
			.HasMaxLength(StringLengths.Email)
			.IsRequired();

		builder.Property(e => e.Password)
			.HasMaxLength(StringLengths.Password)
			.IsRequired();

		builder.Property(e => e.SSOTokenId)
			.HasMaxLength(StringLengths.SSOTokenId)
			.IsRequired();

		builder.HasIndex(e => e.Email)
			.IsUnique();

		builder.Property(e => e.Role)
			.IsRequired();
	}
}
