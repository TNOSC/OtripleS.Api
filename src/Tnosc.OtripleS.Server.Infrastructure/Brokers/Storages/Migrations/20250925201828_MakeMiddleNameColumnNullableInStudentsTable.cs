using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages.Migrations;

/// <inheritdoc />
public partial class MakeMiddleNameColumnNullableInStudentsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => 
        migrationBuilder.AlterColumn<string>(
            name: "MiddleName",
            table: "Students",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(100)",
            oldMaxLength: 100);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => 
        migrationBuilder.AlterColumn<string>(
            name: "MiddleName",
            table: "Students",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(100)",
            oldMaxLength: 100,
            oldNullable: true);
}
