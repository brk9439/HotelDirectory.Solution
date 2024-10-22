using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDirectory.Hotel.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FK_HotelInfo",
                table: "ContactInfo",
                newName: "HotelInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_HotelInfoId",
                table: "ContactInfo",
                column: "HotelInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_HotelInfo_HotelInfoId",
                table: "ContactInfo",
                column: "HotelInfoId",
                principalTable: "HotelInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_HotelInfo_HotelInfoId",
                table: "ContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfo_HotelInfoId",
                table: "ContactInfo");

            migrationBuilder.RenameColumn(
                name: "HotelInfoId",
                table: "ContactInfo",
                newName: "FK_HotelInfo");
        }
    }
}
