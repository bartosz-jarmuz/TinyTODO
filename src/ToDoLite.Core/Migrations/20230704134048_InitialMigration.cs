using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoLite.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    ValueBoolean = table.Column<bool>(type: "INTEGER", nullable: true),
                    ValueString = table.Column<string>(type: "TEXT", nullable: true),
                    ValueDouble = table.Column<double>(type: "REAL", nullable: true),
                    ValueInteger = table.Column<int>(type: "INTEGER", nullable: true),
                    ValueDateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDoItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CapturedDataType = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveWindowTitle = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlainText = table.Column<string>(type: "TEXT", nullable: true),
                    RawData = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ToDoItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Bytes = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageData_ToDoItems_ToDoItemId",
                        column: x => x.ToDoItemId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagToDoItem",
                columns: table => new
                {
                    TagsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ToDoItemsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagToDoItem", x => new { x.TagsId, x.ToDoItemsId });
                    table.ForeignKey(
                        name: "FK_TagToDoItem_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagToDoItem_ToDoItems_ToDoItemsId",
                        column: x => x.ToDoItemsId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageData_ToDoItemId",
                table: "ImageData",
                column: "ToDoItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TagToDoItem_ToDoItemsId",
                table: "TagToDoItem",
                column: "ToDoItemsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageData");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "TagToDoItem");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "ToDoItems");
        }
    }
}
