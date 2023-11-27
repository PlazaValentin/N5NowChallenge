using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5NowChallenge.Domain.Entities;

namespace N5NowChallenge.Infrastructure.EntityConfiguration;
public abstract class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(p => p.FirstNameEmployee)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.LastNameEmployee)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.PermissionTypeId)
            .IsRequired();

        builder.Property(p => p.DatePermission)
            .IsRequired();
    }
}