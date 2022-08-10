using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNotesApp.Migrations
{
    public partial class MigracijaProdavnica2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prodavnicas",
                table: "Prodavnicas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_korisniks",
                table: "korisniks");

            migrationBuilder.RenameTable(
                name: "Prodavnicas",
                newName: "Prodavnice");

            migrationBuilder.RenameTable(
                name: "korisniks",
                newName: "Korisnici");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prodavnice",
                table: "Prodavnice",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Korisnici",
                table: "Korisnici",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prodavnice",
                table: "Prodavnice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Korisnici",
                table: "Korisnici");

            migrationBuilder.RenameTable(
                name: "Prodavnice",
                newName: "Prodavnicas");

            migrationBuilder.RenameTable(
                name: "Korisnici",
                newName: "korisniks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prodavnicas",
                table: "Prodavnicas",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_korisniks",
                table: "korisniks",
                column: "id");
        }
    }
}
