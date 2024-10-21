using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDirectory.Hotel.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_HotelInfo_HotelId",
                table: "ContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfo_HotelId",
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

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HotelInfo",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_HotelId",
                table: "ContactInfo",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_HotelInfo_HotelId",
                table: "ContactInfo",
                column: "HotelId",
                principalTable: "HotelInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
