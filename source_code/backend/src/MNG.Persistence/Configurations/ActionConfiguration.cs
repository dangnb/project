using MNG.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = MNG.Domain.Entities.Identity.Action;

namespace MNG.Persistence.Configurations;
public class ActionConfiguration : IEntityTypeConfiguration<Action>
{
    public void Configure(EntityTypeBuilder<Action> builder)
    {
        builder.ToTable(TableNames.Actions);

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).HasMaxLength(50);
        builder.Property(t => t.Name).HasMaxLength(200).IsRequired(true);
        builder.Property(t => t.IsActive).HasDefaultValue(true);
        builder.Property(t => t.SortOrder).HasDefaultValue(null);

        //Each User can have many Permission
        builder.HasMany(x => x.Permissions)
            .WithOne()
            .HasForeignKey(t => t.ActionId)
            .IsRequired(true);

        //Each User can have many ActionInFunction
        builder.HasMany(x => x.ActionInFunctions)
            .WithOne()
            .HasForeignKey(t => t.ActionId)
            .IsRequired(true);
    }
}
