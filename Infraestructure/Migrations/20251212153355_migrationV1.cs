using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migrationV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Grapes",
            //    columns: table => new
            //    {
            //        UuId = table.Column<Guid>(type: "uuid", nullable: false),
            //        Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
            //        IsActive = table.Column<bool>(type: "boolean", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Grapes", x => x.UuId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        UuId = table.Column<Guid>(type: "uuid", nullable: false),
            //        Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
            //        PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
            //        FullName = table.Column<string>(type: "text", nullable: true),
            //        RoleUser = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
            //        IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.UuId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Wines",
            //    columns: table => new
            //    {
            //        UuId = table.Column<Guid>(type: "uuid", nullable: false),
            //        Name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
            //        WineryName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
            //        RegionName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
            //        CountryName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
            //        WineType = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
            //        VintageYear = table.Column<int>(type: "integer", nullable: false),
            //        LabelImageUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
            //        TastingNotes = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
            //        Price = table.Column<decimal>(type: "numeric", nullable: true),
            //        AverageScore = table.Column<double>(type: "double precision", nullable: false),
            //        RatingCount = table.Column<int>(type: "integer", nullable: false),
            //        Aroma = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
            //        IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Wines", x => x.UuId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CellarPhysics",
            //    columns: table => new
            //    {
            //        UuId = table.Column<Guid>(type: "uuid", nullable: false),
            //        UserId = table.Column<Guid>(type: "uuid", nullable: false),
            //        Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //        Capacity = table.Column<int>(type: "integer", nullable: true),
            //        IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CellarPhysics", x => x.UuId);
            //        table.ForeignKey(
            //            name: "FK_CellarPhysics_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Ratings",
            //    columns: table => new
            //    {
            //        UuId = table.Column<Guid>(type: "uuid", nullable: false),
            //        UserId = table.Column<Guid>(type: "uuid", nullable: false),
            //        WineId = table.Column<Guid>(type: "uuid", nullable: false),
            //        score = table.Column<int>(type: "integer", nullable: false),
            //        IsPublic = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //        Review = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //        UserUuId = table.Column<Guid>(type: "uuid", nullable: true),
            //        WineUuId = table.Column<Guid>(type: "uuid", nullable: true),
            //        IsActive = table.Column<bool>(type: "boolean", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Ratings", x => x.UuId);
            //        table.CheckConstraint("CK_Rating_Score", "score >= 1 AND score <= 5");
            //        table.ForeignKey(
            //            name: "FK_Ratings_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Ratings_Users_UserUuId",
            //            column: x => x.UserUuId,
            //            principalTable: "Users",
            //            principalColumn: "UuId");
            //        table.ForeignKey(
            //            name: "FK_Ratings_Wines_WineId",
            //            column: x => x.WineId,
            //            principalTable: "Wines",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Ratings_Wines_WineUuId",
            //            column: x => x.WineUuId,
            //            principalTable: "Wines",
            //            principalColumn: "UuId");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "WineFavorites",
            //    columns: table => new
            //    {
            //        UserId = table.Column<Guid>(type: "uuid", nullable: false),
            //        WineId = table.Column<Guid>(type: "uuid", nullable: false),
            //        UuId = table.Column<Guid>(type: "uuid", nullable: false),
            //        IsActive = table.Column<bool>(type: "boolean", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WineFavorites", x => new { x.UserId, x.WineId });
            //        table.ForeignKey(
            //            name: "FK_WineFavorites_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_WineFavorites_Wines_WineId",
            //            column: x => x.WineId,
            //            principalTable: "Wines",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "WineGrapeVarieties",
            //    columns: table => new
            //    {
            //        GrapeId = table.Column<Guid>(type: "uuid", nullable: false),
            //        WineId = table.Column<Guid>(type: "uuid", nullable: false),
            //        percentage = table.Column<int>(type: "integer", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WineGrapeVarieties", x => new { x.WineId, x.GrapeId });
            //        table.CheckConstraint("CK_WineGrapeVariety_Percentage", "percentage IS NULL OR (percentage >= 0 AND percentage <= 100)");
            //        table.ForeignKey(
            //            name: "FK_WineGrapeVarieties_Grapes_GrapeId",
            //            column: x => x.GrapeId,
            //            principalTable: "Grapes",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_WineGrapeVarieties_Wines_WineId",
            //            column: x => x.WineId,
            //            principalTable: "Wines",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "WineUsers",
            //    columns: table => new
            //    {
            //        UuId = table.Column<Guid>(type: "uuid", nullable: false),
            //        UserId = table.Column<Guid>(type: "uuid", nullable: false),
            //        WineId = table.Column<Guid>(type: "uuid", nullable: false),
            //        TastingNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
            //        TimesConsumed = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
            //        LastConsumedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //        IsActive = table.Column<bool>(type: "boolean", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WineUsers", x => x.UuId);
            //        table.ForeignKey(
            //            name: "FK_WineUsers_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_WineUsers_Wines_WineId",
            //            column: x => x.WineId,
            //            principalTable: "Wines",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "WineUserCellarItems",
            //    columns: table => new
            //    {
            //        UuId = table.Column<Guid>(type: "uuid", nullable: false),
            //        CellarPhysicsId = table.Column<Guid>(type: "uuid", nullable: false),
            //        WineId = table.Column<Guid>(type: "uuid", nullable: false),
            //        quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
            //        LocationNote = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
            //        DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
            //        IsActive = table.Column<bool>(type: "boolean", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WineUserCellarItems", x => x.UuId);
            //        table.CheckConstraint("CK_CellarItem_Quantity", "quantity > 0");
            //        table.ForeignKey(
            //            name: "FK_WineUserCellarItems_CellarPhysics_CellarPhysicsId",
            //            column: x => x.CellarPhysicsId,
            //            principalTable: "CellarPhysics",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_WineUserCellarItems_Wines_WineId",
            //            column: x => x.WineId,
            //            principalTable: "Wines",
            //            principalColumn: "UuId",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CellarPhysics_UserId",
            //    table: "CellarPhysics",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Grapes_Name",
            //    table: "Grapes",
            //    column: "Name",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Ratings_UserId_WineId",
            //    table: "Ratings",
            //    columns: new[] { "UserId", "WineId" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Ratings_UserUuId",
            //    table: "Ratings",
            //    column: "UserUuId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Ratings_WineId",
            //    table: "Ratings",
            //    column: "WineId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Ratings_WineUuId",
            //    table: "Ratings",
            //    column: "WineUuId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Users_Email",
            //    table: "Users",
            //    column: "Email",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_WineFavorites_WineId",
            //    table: "WineFavorites",
            //    column: "WineId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_WineGrapeVarieties_GrapeId",
            //    table: "WineGrapeVarieties",
            //    column: "GrapeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Wines_Name_VintageYear",
            //    table: "Wines",
            //    columns: new[] { "Name", "VintageYear" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_WineUserCellarItems_CellarPhysicsId_WineId",
            //    table: "WineUserCellarItems",
            //    columns: new[] { "CellarPhysicsId", "WineId" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_WineUserCellarItems_WineId",
            //    table: "WineUserCellarItems",
            //    column: "WineId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_WineUsers_UserId_WineId",
            //    table: "WineUsers",
            //    columns: new[] { "UserId", "WineId" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_WineUsers_WineId",
            //    table: "WineUsers",
            //    column: "WineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "WineFavorites");

            migrationBuilder.DropTable(
                name: "WineGrapeVarieties");

            migrationBuilder.DropTable(
                name: "WineUserCellarItems");

            migrationBuilder.DropTable(
                name: "WineUsers");

            migrationBuilder.DropTable(
                name: "Grapes");

            migrationBuilder.DropTable(
                name: "CellarPhysics");

            migrationBuilder.DropTable(
                name: "Wines");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
