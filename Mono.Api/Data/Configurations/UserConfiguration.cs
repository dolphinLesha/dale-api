using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Api.Domain;

namespace Mono.Api.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Guid);

        builder.Property(user => user.Guid)
            .HasColumnName("guid")
            .ValueGeneratedNever();

        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasMaxLength(254)
            .IsRequired();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(user => user.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(user => user.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(user => user.Surname)
            .HasColumnName("surname")
            .HasMaxLength(50);

        builder.Property(user => user.Birthday)
            .HasColumnName("birthday")
            .HasColumnType("date");

        builder.Property(user => user.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

        builder.Property(user => user.Created)
            .HasColumnName("created")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(user => user.Updated)
            .HasColumnName("updated")
            .HasColumnType("timestamp with time zone");

        builder.Property(user => user.Deleted)
            .HasColumnName("deleted")
            .HasColumnType("timestamp with time zone");
    }
}