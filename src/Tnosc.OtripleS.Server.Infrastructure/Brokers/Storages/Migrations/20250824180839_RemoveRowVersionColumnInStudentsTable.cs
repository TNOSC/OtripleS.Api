// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages.Migrations;

/// <inheritdoc />
public partial class RemoveRowVersionColumnInStudentsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => 
        migrationBuilder.DropColumn(
            name: "RowVersion",
            table: "Students");

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => 
        migrationBuilder.AddColumn<byte[]>(
            name: "RowVersion",
            table: "Students",
            type: "rowversion",
            rowVersion: true,
            nullable: false,
            defaultValue: System.Array.Empty<byte>());
}
