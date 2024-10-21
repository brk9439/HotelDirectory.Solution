using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDirectory.Hotel.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HotelInfoId",
                table: "ContactInfo",
                newName: "FK_HotelInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FK_HotelInfo",
                table: "ContactInfo",
                newName: "HotelInfoId");
        }
    }
}
