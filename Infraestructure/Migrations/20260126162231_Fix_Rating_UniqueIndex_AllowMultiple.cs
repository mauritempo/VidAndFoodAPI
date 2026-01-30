using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Rating_UniqueIndex_AllowMultiple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserUuId_WineUuId",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUuId_WineUuId_IsActive",
                table: "Ratings",
                columns: new[] { "UserUuId", "WineUuId", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserUuId_WineUuId_IsActive",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUuId_WineUuId",
                table: "Ratings",
                columns: new[] { "UserUuId", "WineUuId" },
                unique: true);
        }
    }
}
