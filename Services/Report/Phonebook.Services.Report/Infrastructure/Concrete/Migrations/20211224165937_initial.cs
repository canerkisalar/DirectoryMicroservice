using System;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;


namespace Phonebook.Services.Report.Infrastructure.Concrete.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phonebooks",
                columns: table => new
                {
                    PhonebookId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Company = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phonebooks", x => x.PhonebookId);
                });

            migrationBuilder.CreateTable(
                name: "ReportHead",
                columns: table => new
                {
                    ReportHeadId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PreparationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportHead", x => x.ReportHeadId);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactType = table.Column<string>(type: "text", nullable: true),
                    ContactInformation = table.Column<string>(type: "text", nullable: true),
                    PhonebookId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contacts_Phonebooks_PhonebookId",
                        column: x => x.PhonebookId,
                        principalTable: "Phonebooks",
                        principalColumn: "PhonebookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportItem",
                columns: table => new
                {
                    ReportItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ReportHeadId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportItem", x => x.ReportItemId);
                    table.ForeignKey(
                        name: "FK_ReportItem_ReportHead_ReportHeadId",
                        column: x => x.ReportHeadId,
                        principalTable: "ReportHead",
                        principalColumn: "ReportHeadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PhonebookId",
                table: "Contacts",
                column: "PhonebookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportItem_ReportHeadId",
                table: "ReportItem",
                column: "ReportHeadId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "ReportItem");

            migrationBuilder.DropTable(
                name: "Phonebooks");

            migrationBuilder.DropTable(
                name: "ReportHead");
        }
    }
}
