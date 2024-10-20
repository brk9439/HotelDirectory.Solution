using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDirectory.Hotel.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FK_HotelInfo = table.Column<Guid>(type: "uuid", nullable: false),
                    InfoType = table.Column<int>(type: "integer", nullable: false),
                    InfoContent = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonName = table.Column<string>(type: "text", nullable: false),
                    PersonSurname = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelInfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "HotelInfo");
        }
    }
}
