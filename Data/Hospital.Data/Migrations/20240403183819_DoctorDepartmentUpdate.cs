using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Data.Migrations
{
    /// <inheritdoc />
    public partial class DoctorDepartmentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Doctors",
                newName: "BossDepartmentId");

            migrationBuilder.CreateTable(
                name: "DoctorDepartments",
                columns: table => new
                {
                    DepartmentsDepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorDepartments", x => new { x.DepartmentsDepartmentId, x.DoctorsId });
                    table.ForeignKey(
                        name: "FK_DoctorDepartments_Departments_DepartmentsDepartmentId",
                        column: x => x.DepartmentsDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorDepartments_Doctors_DoctorsId",
                        column: x => x.DoctorsId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_BossDepartmentId",
                table: "Doctors",
                column: "BossDepartmentId",
                unique: true,
                filter: "[BossDepartmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDepartments_DoctorsId",
                table: "DoctorDepartments",
                column: "DoctorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_BossDepartmentId",
                table: "Doctors",
                column: "BossDepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_BossDepartmentId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "DoctorDepartments");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_BossDepartmentId",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "BossDepartmentId",
                table: "Doctors",
                newName: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }
    }
}
