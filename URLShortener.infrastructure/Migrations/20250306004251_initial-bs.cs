using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShortener.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialbs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortenedUrls",
                columns: table => new
                {
                    shortUrl_id = table.Column<Guid>(type: "uuid", nullable: false),
                    longUrl = table.Column<string>(type: "text", nullable: false),
                    shortUrl = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortenedUrls", x => x.shortUrl_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrls_code",
                table: "ShortenedUrls",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortenedUrls");
        }
    }
}
