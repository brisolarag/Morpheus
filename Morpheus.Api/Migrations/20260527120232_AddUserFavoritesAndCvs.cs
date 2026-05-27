using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Morpheus.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFavoritesAndCvs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCvs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileData = table.Column<byte[]>(type: "bytea", nullable: false),
                    ExtractedText = table.Column<string>(type: "text", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCvs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoriteJobs",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    FavoritedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteJobs", x => new { x.UserId, x.JobId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteJobs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteJobs_JobId",
                table: "UserFavoriteJobs",
                column: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCvs");

            migrationBuilder.DropTable(
                name: "UserFavoriteJobs");
        }
    }
}
