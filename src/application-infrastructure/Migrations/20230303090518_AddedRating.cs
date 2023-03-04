using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace application_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WatchLaters",
                keyColumn: "Id",
                keyValue: new Guid("b25a433b-1c0d-46e6-b288-8b7863229810"));

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    RatingValue = table.Column<double>(type: "double precision", nullable: false),
                    RatedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DeleteData(
                table: "WatchLaters",
                keyColumn: "Id",
                keyValue: new Guid("f639f5c3-ac35-4ffc-9e9d-09dfcb065349"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4146a38c-9f4e-4cf9-acff-175d745f0e54",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "030d0647-647f-456e-ae84-48e5648d652a", "39f08b15-cc03-4135-ac30-cd4c7ed81c99" });

            migrationBuilder.InsertData(
                table: "WatchLaters",
                columns: new[] { "Id", "ProgramId", "ProgramName", "UserId" },
                values: new object[] { new Guid("b25a433b-1c0d-46e6-b288-8b7863229810"), 1000, "Avatar", "4146a38c-9f4e-4cf9-acff-175d745f0e54" });
        }
    }
}
