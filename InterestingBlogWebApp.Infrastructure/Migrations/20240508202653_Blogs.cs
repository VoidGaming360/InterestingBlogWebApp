using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterestingBlogWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Blogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURI",
                table: "Blog");

            migrationBuilder.AddColumn<int>(
                name: "DownVoteCount",
                table: "Blog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Image",
                table: "Blog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Blog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpVoteCount",
                table: "Blog",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownVoteCount",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "UpVoteCount",
                table: "Blog");

            migrationBuilder.AddColumn<string>(
                name: "ImageURI",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
