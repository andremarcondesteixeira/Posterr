using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Posterr.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "VARCHAR(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorUsername = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    OriginalPostId = table.Column<long>(type: "bigint", nullable: true),
                    OriginalPostAuthorId = table.Column<long>(type: "bigint", nullable: true),
                    OriginalPostAuthorUsername = table.Column<string>(type: "text", nullable: true),
                    OriginalPostContent = table.Column<string>(type: "text", nullable: true),
                    OriginalPostPublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_Publications_OriginalPostId",
                        column: x => x.OriginalPostId,
                        principalTable: "Publications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Publications_Users_OriginalPostId",
                        column: x => x.OriginalPostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Username" },
                values: new object[,]
                {
                    { 1L, "simba" },
                    { 2L, "nala" },
                    { 3L, "timon" },
                    { 4L, "pumbaa" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publications_OriginalPostId",
                table: "Publications",
                column: "OriginalPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
