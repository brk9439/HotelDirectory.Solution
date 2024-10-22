using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDirectory.Hotel.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_HotelInfo_HotelInfoId",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "HotelInfo");

            migrationBuilder.RenameColumn(
                name: "InfoType",
                table: "ContactInfo",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "InfoContent",
                table: "ContactInfo",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "HotelInfoId",
                table: "ContactInfo",
                newName: "HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactInfo_HotelInfoId",
                table: "ContactInfo",
                newName: "IX_ContactInfo_HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_HotelInfo_HotelId",
                table: "ContactInfo",
                column: "HotelId",
                principalTable: "HotelInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_HotelInfo_HotelId",
                table: "ContactInfo");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ContactInfo",
                newName: "InfoType");

            migrationBuilder.RenameColumn(
                name: "HotelId",
                table: "ContactInfo",
                newName: "HotelInfoId");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "ContactInfo",
                newName: "InfoContent");

            migrationBuilder.RenameIndex(
                name: "IX_ContactInfo_HotelId",
                table: "ContactInfo",
                newName: "IX_ContactInfo_HotelInfoId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HotelInfo",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_HotelInfo_HotelInfoId",
                table: "ContactInfo",
                column: "HotelInfoId",
                principalTable: "HotelInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
