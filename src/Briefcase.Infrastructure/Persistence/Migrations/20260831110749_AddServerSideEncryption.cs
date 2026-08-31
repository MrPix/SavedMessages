using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Briefcase.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServerSideEncryption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NavigationLongitude",
                table: "Messages",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NavigationLatitude",
                table: "Messages",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsServerEncrypted",
                table: "Messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEncrypted",
                table: "FileAttachments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsServerEncrypted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsEncrypted",
                table: "FileAttachments");

            migrationBuilder.AlterColumn<double>(
                name: "NavigationLongitude",
                table: "Messages",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "NavigationLatitude",
                table: "Messages",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
