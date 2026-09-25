using Dale.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dale.Api.Data.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(token => token.Guid);

        builder.Property(token => token.Guid)
            .HasColumnName("guid")
            .ValueGeneratedNever();

        builder.Property(token => token.UserGuid)
            .HasColumnName("user")
            .IsRequired();

        builder.Property(token => token.TokenHash)
            .HasColumnName("token_hash")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(token => token.Expires)
            .HasColumnName("expires")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(token => token.Created)
            .HasColumnName("created")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(token => token.Used)
            .HasColumnName("used")
            .HasColumnType("timestamp with time zone");

        builder.Property(token => token.Revoked)
            .HasColumnName("revoked")
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(token => token.TokenHash)
            .IsUnique();

        builder.HasIndex(token => token.UserGuid);

        builder.HasOne(token => token.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(token => token.UserGuid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}