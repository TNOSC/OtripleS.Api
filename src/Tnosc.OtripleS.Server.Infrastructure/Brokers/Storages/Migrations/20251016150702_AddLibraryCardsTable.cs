// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages.Migrations;

/// <inheritdoc />
public partial class AddLibraryCardsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "LibraryCards",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LibraryAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LibraryCards", x => x.Id);
                table.ForeignKey(
                    name: "FK_LibraryCards_LibraryAccounts_LibraryAccountId",
                    column: x => x.LibraryAccountId,
                    principalTable: "LibraryAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_LibraryCards_LibraryAccountId",
            table: "LibraryCards",
            column: "LibraryAccountId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropTable(
            name: "LibraryCards");
}
