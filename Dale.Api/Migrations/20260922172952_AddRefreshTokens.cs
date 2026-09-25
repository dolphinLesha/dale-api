using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dale.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    user = table.Column<Guid>(type: "uuid", nullable: false),
                    token_hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    expires = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    used = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    revoked = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.guid);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_users_user",
                        column: x => x.user,
                        principalTable: "users",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_token_hash",
                table: "refresh_tokens",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user",
                table: "refresh_tokens",
                column: "user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");
        }
    }
}
