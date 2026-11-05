using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAdmin.Migrations
{
    /// <inheritdoc />
    public partial class finalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "AudienceRatingId",
                table: "Movie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AudienceRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudienceRating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CriticReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriticReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriticReview_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_AudienceRatingId",
                table: "Movie",
                column: "AudienceRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_CriticReview_MovieId",
                table: "CriticReview",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_AudienceRating_AudienceRatingId",
                table: "Movie",
                column: "AudienceRatingId",
                principalTable: "AudienceRating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_AudienceRating_AudienceRatingId",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "AudienceRating");

            migrationBuilder.DropTable(
                name: "CriticReview");

            migrationBuilder.DropIndex(
                name: "IX_Movie_AudienceRatingId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "AudienceRatingId",
                table: "Movie");

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Movie",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }
    }
}
