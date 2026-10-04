using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArchGen.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Project : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    NomeProjeto = table.Column<string>(type: "TEXT", nullable: false),
                    TipoDoProjeto = table.Column<string>(type: "TEXT", nullable: false),
                    PathSolucao = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Domain_PathEntities = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Domain_PathExceptions = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Domain_PathEnums = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Domain_PathInterfaces = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Application_PathDTOs = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Application_PathUseCases = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Application_PathInterfaces = table.Column<string>(type: "TEXT", nullable: false),
                    paths_InfraStructure_PathData = table.Column<string>(type: "TEXT", nullable: false),
                    paths_InfraStructure_PathRepository = table.Column<string>(type: "TEXT", nullable: false),
                    paths_InfraStructure_PathServices = table.Column<string>(type: "TEXT", nullable: false),
                    paths_InfraStructure_PathMigrations = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Tests_DomainPathTests = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Tests_ApplicationPathTests = table.Column<string>(type: "TEXT", nullable: false),
                    paths_Console_OR_API_ControllerArquivo = table.Column<string>(type: "TEXT", nullable: false),
                    paths_PathDomain = table.Column<string>(type: "TEXT", nullable: false),
                    paths_PathApplication = table.Column<string>(type: "TEXT", nullable: false),
                    paths_PathInfraStructure = table.Column<string>(type: "TEXT", nullable: false),
                    paths_PathTests = table.Column<string>(type: "TEXT", nullable: false),
                    paths_PathConsole_OR_API = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Template");
        }
    }
}
