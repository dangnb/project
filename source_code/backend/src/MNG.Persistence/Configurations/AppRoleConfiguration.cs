using MNG.Domain.Entities.Identity;
using MNG.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNG.Persistence.Configurations;
public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable(TableNames.AppRoles);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description)
            .HasMaxLength(250)
            .IsRequired(true);

        builder.Property(x => x.RoleCode)
            .HasMaxLength(50)
            .IsRequired(true);

        //Each user can have many RoleClaim
        builder.HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(uc => uc.RoleId)
            .IsRequired(true);

        //Earch User can have many entries in the UserRole join table
        builder.HasMany(x => x.UserRoles)
            .WithOne()
            .HasForeignKey(uc => uc.RoleId)
            .IsRequired(true);

        //Earch user can have many Permission
        builder.HasMany(e => e.Permissions)
            .WithOne()
            .HasForeignKey(x => x.RoleId)
            .IsRequired(true);

    }
}
