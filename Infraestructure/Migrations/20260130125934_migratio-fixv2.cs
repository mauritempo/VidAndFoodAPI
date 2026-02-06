using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migratiofixv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserUuId_WineUuId_IsActive",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "Ratings",
                newName: "IsSommelier");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUuId_WineUuId_IsSommelier",
                table: "Ratings",
                columns: new[] { "UserUuId", "WineUuId", "IsSommelier" },
                unique: true,
                filter: "\"IsActive\" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserUuId_WineUuId_IsSommelier",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "IsSommelier",
                table: "Ratings",
                newName: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUuId_WineUuId_IsActive",
                table: "Ratings",
                columns: new[] { "UserUuId", "WineUuId", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = true");
        }
    }
}
