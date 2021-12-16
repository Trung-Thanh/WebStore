using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ProductTranslations",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProductTranslations",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeature",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            //migrationBuilder.UpdateData(
            //    table: "AppRoles",
            //    keyColumn: "Id",
            //    keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
            //    column: "ConcurrencyStamp",
            //    value: "92892222-7c17-4674-b485-2040bc8e23ea");

            //migrationBuilder.UpdateData(
            //    table: "AppUsers",
            //    keyColumn: "Id",
            //    keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
            //    columns: new[] { "ConcurrencyStamp", "PasswordHash" },
            //    values: new object[] { "4a207b4d-301c-4848-8db5-e3f5c098e829", "AQAAAAEAACcQAAAAEHJZU4oJbQdXmBaGuPh2TX04wcM/l1L1CdhRTANQmY+1E6VitPHKTXGkp9rtdb/EsA==" });

            //migrationBuilder.UpdateData(
            //    table: "Products",
            //    keyColumn: "Id",
            //    keyValue: 1,
            //    columns: new[] { "DateCreated", "IsFeature" },
            //    values: new object[] { new DateTime(2021, 12, 1, 21, 45, 8, 597, DateTimeKind.Local).AddTicks(148), false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ProductTranslations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProductTranslations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(800)",
                oldMaxLength: 800,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeature",
                table: "Products",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8ae88023-82c8-4fb5-bb4c-2556b8cb02f9", "AQAAAAEAACcQAAAAEG5oVDneJ+IFjYW/+fNdZg3zb+7OdVrFvPdi9Camp+g/Huo6nAQrFtvvbbHyWagSgQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "IsFeature" },
                values: new object[] { new DateTime(2021, 11, 26, 16, 50, 12, 882, DateTimeKind.Local).AddTicks(5405), null });
        }
    }
}
