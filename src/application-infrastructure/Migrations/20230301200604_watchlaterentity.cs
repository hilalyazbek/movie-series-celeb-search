using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace application_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class watchlaterentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatchLaters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    ProgramName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchLaters", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "QidNumber", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4146a38c-9f4e-4cf9-acff-175d745f0e54", 0, "030d0647-647f-456e-ae84-48e5648d652a", "User", "batman@gotham.com", false, "Bruce", "Wayne", false, null, null, null, "0b58fb7e73e1402d43e6263a58e0d3db7f237935", null, false, "432432432", "39f08b15-cc03-4135-ac30-cd4c7ed81c99", false, "batman" });

            migrationBuilder.InsertData(
                table: "WatchLaters",
                columns: new[] { "Id", "ProgramId", "ProgramName", "UserId" },
                values: new object[] { new Guid("b25a433b-1c0d-46e6-b288-8b7863229810"), 1000, "Avatar", "4146a38c-9f4e-4cf9-acff-175d745f0e54" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchLaters");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4146a38c-9f4e-4cf9-acff-175d745f0e54");
        }
    }
}
