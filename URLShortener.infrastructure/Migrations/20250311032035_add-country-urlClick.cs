using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShortener.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcountryurlClick : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "urlClicks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "urlClicks");
        }
    }
}
