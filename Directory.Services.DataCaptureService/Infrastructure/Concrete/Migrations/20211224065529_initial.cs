using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Directory.Services.DataCaptureService.Infrastructure.Concrete.Migrations
{
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

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PhonebookId",
                table: "Contacts",
                column: "PhonebookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Phonebooks");
        }
    }
}
