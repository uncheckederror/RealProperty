﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class CharToStringForSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AFNonProfitUse",
                table: "Sales",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(char),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AFHistoricProperty",
                table: "Sales",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(char),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AFForestLand",
                table: "Sales",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(char),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AFCurrentUseLand",
                table: "Sales",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(char),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<char>(
                name: "AFNonProfitUse",
                table: "Sales",
                type: "TEXT",
                nullable: false,
                defaultValue: ' ',
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<char>(
                name: "AFHistoricProperty",
                table: "Sales",
                type: "TEXT",
                nullable: false,
                defaultValue: ' ',
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<char>(
                name: "AFForestLand",
                table: "Sales",
                type: "TEXT",
                nullable: false,
                defaultValue: ' ',
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<char>(
                name: "AFCurrentUseLand",
                table: "Sales",
                type: "TEXT",
                nullable: false,
                defaultValue: ' ',
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
