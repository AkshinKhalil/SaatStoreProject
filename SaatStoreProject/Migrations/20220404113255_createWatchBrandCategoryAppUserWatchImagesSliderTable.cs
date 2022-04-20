using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SaatStoreProject.Migrations
{
    public partial class createWatchBrandCategoryAppUserWatchImagesSliderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Models_ModelId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Watches_Categories_CategoryId",
                table: "Watches");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Watches_CategoryId",
                table: "Watches");

            migrationBuilder.DropIndex(
                name: "IX_Brands_ModelId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Brands");

            migrationBuilder.AddColumn<string>(
                name: "WatchModel",
                table: "Watches",
                maxLength: 90,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    Image = table.Column<string>(maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(maxLength: 150, nullable: true),
                    SearchIcon = table.Column<string>(nullable: false),
                    NumberIcon = table.Column<string>(nullable: false),
                    BasketIcon = table.Column<string>(nullable: false),
                    LoginIcon = table.Column<string>(nullable: true),
                    Connectnumber = table.Column<int>(maxLength: 150, nullable: false),
                    TwitIcon = table.Column<string>(maxLength: 150, nullable: true),
                    FacebookIcon = table.Column<string>(maxLength: 150, nullable: true),
                    InstagramIcon = table.Column<string>(maxLength: 150, nullable: true),
                    VideoURL = table.Column<string>(maxLength: 250, nullable: true),
                    InfoTxt1 = table.Column<string>(maxLength: 350, nullable: true),
                    InfoTxt2 = table.Column<string>(maxLength: 350, nullable: true),
                    InfoTxt3 = table.Column<string>(maxLength: 350, nullable: true),
                    InfoVideoURL = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WatchId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchCategory_Watches_WatchId",
                        column: x => x.WatchId,
                        principalTable: "Watches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchCategory_CategoryId",
                table: "WatchCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchCategory_WatchId",
                table: "WatchCategory",
                column: "WatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "WatchCategory");

            migrationBuilder.DropColumn(
                name: "WatchModel",
                table: "Watches");

            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "Brands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    WatchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Watches_WatchId",
                        column: x => x.WatchId,
                        principalTable: "Watches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Watches_CategoryId",
                table: "Watches",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_ModelId",
                table: "Brands",
                column: "ModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_WatchId",
                table: "Models",
                column: "WatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Models_ModelId",
                table: "Brands",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Watches_Categories_CategoryId",
                table: "Watches",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
