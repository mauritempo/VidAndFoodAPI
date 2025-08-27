using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
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
                    Name = table.Column<string>(type: "TEXT", nullable: false)
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
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    RoleUser = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
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
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    WineryName = table.Column<string>(type: "TEXT", nullable: false),
                    RegionName = table.Column<string>(type: "TEXT", nullable: false),
                    CountryName = table.Column<string>(type: "TEXT", nullable: false),
                    WyneType = table.Column<int>(type: "INTEGER", nullable: false),
                    VintageYear = table.Column<int>(type: "INTEGER", nullable: false),
                    LabelImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    TastingNotes = table.Column<string>(type: "TEXT", nullable: true),
                    Aroma = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CellarPhysics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CellarPhysics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CellarPhysics_Users_UserId",
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
                    Rate = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WineGrapeVarieties",
                columns: table => new
                {
                    GrapeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WineId = table.Column<int>(type: "INTEGER", nullable: false),
                    GrapeId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Percentage = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineGrapeVarieties", x => x.GrapeId);
                    table.ForeignKey(
                        name: "FK_WineGrapeVarieties_Grapes_GrapeId1",
                        column: x => x.GrapeId1,
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
                name: "WineUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    WineId = table.Column<int>(type: "INTEGER", nullable: false),
                    TastingNotes = table.Column<string>(type: "TEXT", nullable: false),
                    Opinion = table.Column<string>(type: "TEXT", nullable: false),
                    CellearPhysisIdId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WineUsers_CellarPhysics_CellearPhysisIdId",
                        column: x => x.CellearPhysisIdId,
                        principalTable: "CellarPhysics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineUsers_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
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
                    CellarPhysicsId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationNote = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineUserCellarItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WineUserCellarItems_CellarPhysics_CellarPhysicsId",
                        column: x => x.CellarPhysicsId,
                        principalTable: "CellarPhysics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineUserCellarItems_WineUsers_WineUserId",
                        column: x => x.WineUserId,
                        principalTable: "WineUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CellarPhysics_UserId",
                table: "CellarPhysics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_WineId",
                table: "Ratings",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_WineGrapeVarieties_GrapeId1",
                table: "WineGrapeVarieties",
                column: "GrapeId1");

            migrationBuilder.CreateIndex(
                name: "IX_WineGrapeVarieties_WineId",
                table: "WineGrapeVarieties",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_WineUserCellarItems_CellarPhysicsId",
                table: "WineUserCellarItems",
                column: "CellarPhysicsId");

            migrationBuilder.CreateIndex(
                name: "IX_WineUserCellarItems_WineUserId",
                table: "WineUserCellarItems",
                column: "WineUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WineUsers_CellearPhysisIdId",
                table: "WineUsers",
                column: "CellearPhysisIdId");

            migrationBuilder.CreateIndex(
                name: "IX_WineUsers_UserId",
                table: "WineUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WineUsers_WineId",
                table: "WineUsers",
                column: "WineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "WineGrapeVarieties");

            migrationBuilder.DropTable(
                name: "WineUserCellarItems");

            migrationBuilder.DropTable(
                name: "Grapes");

            migrationBuilder.DropTable(
                name: "WineUsers");

            migrationBuilder.DropTable(
                name: "CellarPhysics");

            migrationBuilder.DropTable(
                name: "Wines");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
