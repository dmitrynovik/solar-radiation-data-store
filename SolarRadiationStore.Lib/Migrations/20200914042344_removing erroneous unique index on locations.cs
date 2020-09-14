using Microsoft.EntityFrameworkCore.Migrations;

namespace SolarRadiationStore.Lib.Migrations
{
    public partial class removingerroneousuniqueindexonlocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Created_Modified_Srid",
                table: "Locations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Locations_Created_Modified_Srid",
                table: "Locations",
                columns: new[] { "Created", "Modified", "Srid" },
                unique: true);
        }
    }
}
