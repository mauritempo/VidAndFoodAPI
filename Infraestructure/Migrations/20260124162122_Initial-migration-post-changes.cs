using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigrationpostchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserUuId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Wines_WineId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Wines_WineUuId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId_WineId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserUuId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_WineId",
                table: "Ratings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Rating_Score",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "WineId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "score",
                table: "Ratings",
                newName: "Score");

            migrationBuilder.AlterColumn<Guid>(
                name: "WineUuId",
                table: "Ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserUuId",
                table: "Ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublic",
                table: "Ratings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUuId_WineUuId",
                table: "Ratings",
                columns: new[] { "UserUuId", "WineUuId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserUuId",
                table: "Ratings",
                column: "UserUuId",
                principalTable: "Users",
                principalColumn: "UuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Wines_WineUuId",
                table: "Ratings",
                column: "WineUuId",
                principalTable: "Wines",
                principalColumn: "UuId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserUuId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Wines_WineUuId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserUuId_WineUuId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Ratings",
                newName: "score");

            migrationBuilder.AlterColumn<Guid>(
                name: "WineUuId",
                table: "Ratings",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserUuId",
                table: "Ratings",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublic",
                table: "Ratings",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WineId",
                table: "Ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_WineId",
                table: "Ratings",
                columns: new[] { "UserId", "WineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUuId",
                table: "Ratings",
                column: "UserUuId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_WineId",
                table: "Ratings",
                column: "WineId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Rating_Score",
                table: "Ratings",
                sql: "score >= 1 AND score <= 5");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserId",
                table: "Ratings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserUuId",
                table: "Ratings",
                column: "UserUuId",
                principalTable: "Users",
                principalColumn: "UuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Wines_WineId",
                table: "Ratings",
                column: "WineId",
                principalTable: "Wines",
                principalColumn: "UuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Wines_WineUuId",
                table: "Ratings",
                column: "WineUuId",
                principalTable: "Wines",
                principalColumn: "UuId");
        }
    }
}
