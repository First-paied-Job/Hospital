using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospitalId",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_HospitalId",
                table: "Departments",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Hospitals_HospitalId",
                table: "Departments",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Hospitals_HospitalId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_HospitalId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Departments");
        }
    }
}
