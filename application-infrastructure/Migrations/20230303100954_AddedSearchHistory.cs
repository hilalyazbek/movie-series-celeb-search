using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace application_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSearchHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WatchLaters",
                keyColumn: "Id",
                keyValue: new Guid("f639f5c3-ac35-4ffc-9e9d-09dfcb065349"));

            migrationBuilder.CreateTable(
                name: "SearchHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Query = table.Column<string>(type: "text", nullable: true),
                    Results = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistory", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4146a38c-9f4e-4cf9-acff-175d745f0e54",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "691a037d-c9b6-4c4a-8617-67e7b9d5d633", "50df63cc-9e62-4fe6-9557-3cafd86fef5c" });

            migrationBuilder.InsertData(
                table: "WatchLaters",
                columns: new[] { "Id", "ProgramId", "ProgramName", "UserId" },
                values: new object[] { new Guid("6e8fea8d-f1c1-4da6-b828-14047a862e12"), 1000, "Avatar", "4146a38c-9f4e-4cf9-acff-175d745f0e54" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchHistory");

            migrationBuilder.DeleteData(
                table: "WatchLaters",
                keyColumn: "Id",
                keyValue: new Guid("6e8fea8d-f1c1-4da6-b828-14047a862e12"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4146a38c-9f4e-4cf9-acff-175d745f0e54",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d0b15404-128f-48be-ac1c-27b2bc96036c", "abc8e48b-b806-4c8e-ac27-365afe3ad43b" });

            migrationBuilder.InsertData(
                table: "WatchLaters",
                columns: new[] { "Id", "ProgramId", "ProgramName", "UserId" },
                values: new object[] { new Guid("f639f5c3-ac35-4ffc-9e9d-09dfcb065349"), 1000, "Avatar", "4146a38c-9f4e-4cf9-acff-175d745f0e54" });
        }
    }
}
