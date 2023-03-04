using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace application_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedandUpdatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WatchLaters",
                keyColumn: "Id",
                keyValue: new Guid("6e8fea8d-f1c1-4da6-b828-14047a862e12"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WatchLaters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "WatchLaters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SearchHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SearchHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Ratings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Ratings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4146a38c-9f4e-4cf9-acff-175d745f0e54",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "dff693b9-27be-4aec-a4d1-9bd1cd8ab2eb", "60346ce4-7275-4807-944d-3df5fd7a592c" });

            migrationBuilder.InsertData(
                table: "WatchLaters",
                columns: new[] { "Id", "CreatedDate", "ProgramId", "ProgramName", "UpdatedDate", "UserId" },
                values: new object[] { new Guid("6ddbaf23-10dd-4500-be94-48468d2b05e1"), new DateTime(2023, 3, 4, 12, 30, 10, 823, DateTimeKind.Utc).AddTicks(150), 1000, "Avatar", new DateTime(2023, 3, 4, 12, 30, 10, 823, DateTimeKind.Utc).AddTicks(150), "4146a38c-9f4e-4cf9-acff-175d745f0e54" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WatchLaters",
                keyColumn: "Id",
                keyValue: new Guid("6ddbaf23-10dd-4500-be94-48468d2b05e1"));

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WatchLaters");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "WatchLaters");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SearchHistory");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SearchHistory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Ratings");

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
    }
}
