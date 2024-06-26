using Microsoft.EntityFrameworkCore.Migrations;

namespace Posterr.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class RepostsCount : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "AmountOfReposts",
            table: "Publications",
            type: "integer",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AmountOfReposts",
            table: "Publications");
    }
}
