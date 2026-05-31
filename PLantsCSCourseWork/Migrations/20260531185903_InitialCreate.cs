using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PLantsCSCourseWork.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "grd");

            migrationBuilder.CreateTable(
                name: "Plants",
                schema: "grd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ScienceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    Photo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "grd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NickName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BioChars",
                schema: "grd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    LeafType = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Root = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Fruit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AmmFruit = table.Column<char>(type: "character(1)", maxLength: 1, nullable: true),
                    Morphology = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioChars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BioChars_Plants_Id",
                        column: x => x.Id,
                        principalSchema: "grd",
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Growths",
                schema: "grd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Ppfd = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Humidity = table.Column<double>(type: "double precision", nullable: false),
                    Ph = table.Column<double>(type: "double precision", nullable: false),
                    Space = table.Column<int>(type: "integer", nullable: false),
                    Soil = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Survivability = table.Column<string>(type: "text", nullable: false),
                    GrowthSpeed = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Climate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Water = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    LandScape = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Growths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Growths_Plants_Id",
                        column: x => x.Id,
                        principalSchema: "grd",
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantRatings",
                schema: "grd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ViewCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    FavoriteCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantRatings_Plants_Id",
                        column: x => x.Id,
                        principalSchema: "grd",
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                schema: "grd",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PlantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => new { x.UserId, x.PlantId });
                    table.ForeignKey(
                        name: "FK_Favorites_Plants_PlantId",
                        column: x => x.PlantId,
                        principalSchema: "grd",
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "grd",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preferences",
                schema: "grd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Climate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Soil = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Space = table.Column<int>(type: "integer", nullable: true),
                    Water = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    LandScape = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferences_Users_Id",
                        column: x => x.Id,
                        principalSchema: "grd",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PlantId",
                schema: "grd",
                table: "Favorites",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_ScienceName",
                schema: "grd",
                table: "Plants",
                column: "ScienceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                schema: "grd",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                schema: "grd",
                table: "Users",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BioChars",
                schema: "grd");

            migrationBuilder.DropTable(
                name: "Favorites",
                schema: "grd");

            migrationBuilder.DropTable(
                name: "Growths",
                schema: "grd");

            migrationBuilder.DropTable(
                name: "PlantRatings",
                schema: "grd");

            migrationBuilder.DropTable(
                name: "Preferences",
                schema: "grd");

            migrationBuilder.DropTable(
                name: "Plants",
                schema: "grd");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "grd");
        }
    }
}
