using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5NowChallenge.Domain.Entities;

namespace N5NowChallenge.Infrastructure.EntityConfiguration;

public abstract class PermissionTypeEntityConfiguration : IEntityTypeConfiguration<PermissionType>
{
    public void Configure(EntityTypeBuilder<PermissionType> builder)
    {
        builder.ToTable("PermissionsType");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(p => p.Description)
            .IsRequired()
        .HasMaxLength(300);

        builder.HasMany(tp => tp.Permissions)
            .WithOne(p => p.PermissionType)
            .HasForeignKey(p => p.PermissionTypeId);
    }
}
