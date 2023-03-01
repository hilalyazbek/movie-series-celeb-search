using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace application_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedWatchLater : Migration
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
                    table.ForeignKey(
                        name: "FK_WatchLaters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "QidNumber", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4146a38c-9f4e-4cf9-acff-175d745f0e54", 0, "1f39800f-91d4-4ccd-a1bc-e30e979394c5", "User", "batman@gotham.com", false, "Bruce", "Wayne", false, null, null, null, "0b58fb7e73e1402d43e6263a58e0d3db7f237935", null, false, "432432432", "ba3e1efb-3ac6-4818-9660-4052608f9c4c", false, "batman" });

            migrationBuilder.InsertData(
                table: "WatchLaters",
                columns: new[] { "Id", "ProgramId", "ProgramName", "UserId" },
                values: new object[] { new Guid("93ab8fe8-4633-4916-94fc-3cd2d4aa8791"), 1000, "Avatar", "4146a38c-9f4e-4cf9-acff-175d745f0e54" });

            migrationBuilder.CreateIndex(
                name: "IX_WatchLaters_UserId",
                table: "WatchLaters",
                column: "UserId");
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
