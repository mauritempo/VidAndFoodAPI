using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_CellarItem_Quantity",
                table: "WineUserCellarItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WineGrapeVariety_Percentage",
                table: "WineGrapeVarieties");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Rating_Score",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "WineUserCellarItems",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "WineGrapeVarieties",
                newName: "percentage");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Ratings",
                newName: "score");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateAdded",
                table: "WineUserCellarItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CellarItem_Quantity",
                table: "WineUserCellarItems",
                sql: "quantity > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WineGrapeVariety_Percentage",
                table: "WineGrapeVarieties",
                sql: "percentage IS NULL OR (percentage >= 0 AND percentage <= 100)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Rating_Score",
                table: "Ratings",
                sql: "score >= 1 AND score <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_CellarItem_Quantity",
                table: "WineUserCellarItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WineGrapeVariety_Percentage",
                table: "WineGrapeVarieties");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Rating_Score",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "WineUserCellarItems",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "percentage",
                table: "WineGrapeVarieties",
                newName: "Percentage");

            migrationBuilder.RenameColumn(
                name: "score",
                table: "Ratings",
                newName: "Score");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateAdded",
                table: "WineUserCellarItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CellarItem_Quantity",
                table: "WineUserCellarItems",
                sql: "\"Quantity\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WineGrapeVariety_Percentage",
                table: "WineGrapeVarieties",
                sql: "(\"Percentage\" IS NULL) OR (\"Percentage\" >= 0 AND \"Percentage\" <= 100)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Rating_Score",
                table: "Ratings",
                sql: "\"Score\" >= 1 AND \"Score\" <= 5");
        }
    }
}
