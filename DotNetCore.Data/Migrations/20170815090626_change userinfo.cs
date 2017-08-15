using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DotNetCore.Data.Migrations
{
    public partial class changeuserinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserInfo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserInfo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "Address",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserInfoId",
                table: "Address",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_UserInfo_UserInfoId",
                table: "Address",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_UserInfo_UserInfoId",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_UserInfoId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "UserInfo",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
