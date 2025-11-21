using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialmigr1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grapes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grapes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: true),
                    RoleUser = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    WineryName = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    RegionName = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    CountryName = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    WineType = table.Column<string>(type: "TEXT", maxLength: 24, nullable: false),
                    VintageYear = table.Column<int>(type: "INTEGER", nullable: false),
                    LabelImageUrl = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    TastingNotes = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Aroma = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WineUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TastingNotes = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Opinion = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true),
                    isCellarActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    TypeCellar = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WineUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    WineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rate = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.CheckConstraint("CK_Rating_Rate", "[Rate] >= 0 AND [Rate] <= 5");
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WineFavorites",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    WineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineFavorites", x => new { x.UserId, x.WineId });
                    table.ForeignKey(
                        name: "FK_WineFavorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineFavorites_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WineGrapeVarieties",
                columns: table => new
                {
                    GrapeId = table.Column<int>(type: "INTEGER", nullable: false),
                    WineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Percentage = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineGrapeVarieties", x => new { x.WineId, x.GrapeId });
                    table.CheckConstraint("CK_WineGrapeVariety_Percentage", "([Percentage] IS NULL) OR ([Percentage] >= 0 AND [Percentage] <= 100)");
                    table.ForeignKey(
                        name: "FK_WineGrapeVarieties_Grapes_GrapeId",
                        column: x => x.GrapeId,
                        principalTable: "Grapes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineGrapeVarieties_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CellarPhysics",
                columns: table => new
                {
                    WineUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CellarPhysics", x => x.WineUserId);
                    table.ForeignKey(
                        name: "FK_CellarPhysics_WineUsers_WineUserId",
                        column: x => x.WineUserId,
                        principalTable: "WineUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WineUserCellarItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WineUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    WineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    LocationNote = table.Column<string>(type: "TEXT", maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineUserCellarItems", x => x.Id);
                    table.CheckConstraint("CK_WineUserCellarItem_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_WineUserCellarItems_WineUsers_WineUserId",
                        column: x => x.WineUserId,
                        principalTable: "WineUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineUserCellarItems_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grapes_Name",
                table: "Grapes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_WineId",
                table: "Ratings",
                columns: new[] { "UserId", "WineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_WineId",
                table: "Ratings",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WineFavorites_WineId",
                table: "WineFavorites",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_WineGrapeVarieties_GrapeId",
                table: "WineGrapeVarieties",
                column: "GrapeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_Name_VintageYear",
                table: "Wines",
                columns: new[] { "Name", "VintageYear" });

            migrationBuilder.CreateIndex(
                name: "IX_WineUserCellarItems_WineId",
                table: "WineUserCellarItems",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_WineUserCellarItems_WineUserId_WineId",
                table: "WineUserCellarItems",
                columns: new[] { "WineUserId", "WineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WineUsers_UserId",
                table: "WineUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CellarPhysics");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "WineFavorites");

            migrationBuilder.DropTable(
                name: "WineGrapeVarieties");

            migrationBuilder.DropTable(
                name: "WineUserCellarItems");

            migrationBuilder.DropTable(
                name: "Grapes");

            migrationBuilder.DropTable(
                name: "WineUsers");

            migrationBuilder.DropTable(
                name: "Wines");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
