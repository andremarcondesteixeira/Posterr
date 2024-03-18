using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posterr.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(name: "Users",
                                     columns: table => new
                                     {
                                         Id = table.Column<long>(type: "bigint", nullable: false)
                                                   .Annotation("SqlServer:Identity", "1, 1"),
                                         Username = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                                         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                                     },
                                     constraints: table =>
                                     {
                                         table.PrimaryKey("PK_Users", x => x.Id);
                                     });

        migrationBuilder.CreateTable(name: "Posts",
                                     columns: table => new
                                     {
                                         Id = table.Column<long>(type: "bigint", nullable: false)
                                                   .Annotation("SqlServer:Identity", "1, 1"),
                                         UserId = table.Column<long>(type: "bigint", nullable: false),
                                         Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                                         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                                     },
                                     constraints: table =>
                                     {
                                         table.PrimaryKey("PK_Posts", x => x.Id);
                                         table.ForeignKey(name: "FK_Posts_Users_UserId",
                                                          column: x => x.UserId,
                                                          principalTable: "Users",
                                                          principalColumn: "Id",
                                                          onDelete: ReferentialAction.Restrict);
                                     });

        migrationBuilder.CreateTable(name: "Reposts",
                                     columns: table => new
                                     {
                                         UserId = table.Column<long>(type: "bigint", nullable: false),
                                         PostId = table.Column<long>(type: "bigint", nullable: false),
                                         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                                     },
                                     constraints: table =>
                                     {
                                         table.PrimaryKey("PK_Reposts", x => new { x.UserId, x.PostId });
                                         table.ForeignKey(name: "FK_Reposts_Posts_PostId",
                                                          column: x => x.PostId,
                                                          principalTable: "Posts",
                                                          principalColumn: "Id",
                                                          onDelete: ReferentialAction.Restrict);
                                         table.ForeignKey(name: "FK_Reposts_Users_UserId",
                                                          column: x => x.UserId,
                                                          principalTable: "Users",
                                                          principalColumn: "Id",
                                                          onDelete: ReferentialAction.Restrict);
                                     });

        migrationBuilder.InsertData(table: "Users",
                                    columns: ["Id", "CreatedAt", "Username"],
                                    values: new object[,]
                                    {
                                        { 1L, DateTime.UtcNow, "simba" },
                                        { 2L, DateTime.UtcNow, "nala" },
                                        { 3L, DateTime.UtcNow, "timon" },
                                        { 4L, DateTime.UtcNow, "pumbaa" }
                                    });

        migrationBuilder.CreateIndex(name: "IX_Posts_UserId",
                                     table: "Posts",
                                     column: "UserId");

        migrationBuilder.CreateIndex(name: "IX_Reposts_PostId",
                                     table: "Reposts",
                                     column: "PostId");

        migrationBuilder.CreateIndex(name: "IX_Users_Username",
                                     table: "Users",
                                     column: "Username",
                                     unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Reposts");
        migrationBuilder.DropTable(name: "Posts");
        migrationBuilder.DropTable(name: "Users");
    }
}
