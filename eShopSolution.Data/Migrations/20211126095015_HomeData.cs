using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class HomeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductTranslations_Languages_LanguageId1",
            //    table: "ProductTranslations");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProductTranslations_LanguageId1",
            //    table: "ProductTranslations");

            //migrationBuilder.DropColumn(
            //    name: "LanguageId1",
            //    table: "ProductTranslations");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeature",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Slide",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slide", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "0c2eba9d-8a63-49cc-87c9-261c24f99c89");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "8ae88023-82c8-4fb5-bb4c-2556b8cb02f9", "vvakemyfresh@gmail.com", "vvakemyfresh@gmail.com", "AQAAAAEAACcQAAAAEG5oVDneJ+IFjYW/+fNdZg3zb+7OdVrFvPdi9Camp+g/Huo6nAQrFtvvbbHyWagSgQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 11, 26, 16, 50, 12, 882, DateTimeKind.Local).AddTicks(5405));

            migrationBuilder.InsertData(
                table: "Slide",
                columns: new[] { "Id", "Description", "Image", "Name", "SortOrder", "Status", "Url" },
                values: new object[,]
                {
                    { 1, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/1.png", "Second Thumbnail label", 1, 1, "#" },
                    { 2, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/2.png", "Second Thumbnail label", 2, 1, "#" },
                    { 3, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/3.png", "Second Thumbnail label", 3, 1, "#" },
                    { 4, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/4.png", "Second Thumbnail label", 4, 1, "#" },
                    { 5, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/5.png", "Second Thumbnail label", 5, 1, "#" },
                    { 6, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/6.png", "Second Thumbnail label", 6, 1, "#" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slide");

            migrationBuilder.DropColumn(
                name: "IsFeature",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "LanguageId1",
                table: "ProductTranslations",
                type: "varchar(5)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "9c1918c0-5bb0-48eb-aca6-23d50f13c278");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "92dcd6eb-74eb-4387-8dde-636956fe679c", "tedu.international@gmail.com", "tedu.international@gmail.com", "AQAAAAEAACcQAAAAELZaV3KZBknJdrenuHF4UHE4v/wbFyrmxu9Bx0vnkiF/JzCBFTyQAs/LQ3b4LTDn3g==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 7, 30, 18, 31, 13, 927, DateTimeKind.Local).AddTicks(3321));

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_LanguageId1",
                table: "ProductTranslations",
                column: "LanguageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTranslations_Languages_LanguageId1",
                table: "ProductTranslations",
                column: "LanguageId1",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
