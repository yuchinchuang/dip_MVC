using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_GrandeTravel.Migrations
{
    public partial class modifyFeedbackPackageDatabaseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "TblFeedback",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TblFeedback_PackageId",
                table: "TblFeedback",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblFeedback_TblPackage_PackageId",
                table: "TblFeedback",
                column: "PackageId",
                principalTable: "TblPackage",
                principalColumn: "PackageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblFeedback_TblPackage_PackageId",
                table: "TblFeedback");

            migrationBuilder.DropIndex(
                name: "IX_TblFeedback_PackageId",
                table: "TblFeedback");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "TblFeedback");
        }
    }
}
