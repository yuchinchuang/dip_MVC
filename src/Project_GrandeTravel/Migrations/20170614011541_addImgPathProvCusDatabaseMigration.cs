using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_GrandeTravel.Migrations
{
    public partial class addImgPathProvCusDatabaseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "TblProviderProfile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "TblCustomerProfile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "TblProviderProfile");

            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "TblCustomerProfile");
        }
    }
}
