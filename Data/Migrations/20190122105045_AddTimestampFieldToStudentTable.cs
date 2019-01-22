using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF_ConcurrencyTest.Data.Migrations
{
    public partial class AddTimestampFieldToStudentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Students",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Students");
        }
    }
}
