using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArchGen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_PathInTheBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathApplication",
                table: "DataUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PathDomain",
                table: "DataUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PathInfrastructure",
                table: "DataUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PathTests",
                table: "DataUser",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathApplication",
                table: "DataUser");

            migrationBuilder.DropColumn(
                name: "PathDomain",
                table: "DataUser");

            migrationBuilder.DropColumn(
                name: "PathInfrastructure",
                table: "DataUser");

            migrationBuilder.DropColumn(
                name: "PathTests",
                table: "DataUser");
        }
    }
}
