using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgresWpfDemo.Migrations
{
    public partial class InitialBaseline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Databáze a tabulka users už existují.
            // Tato migrace pouze označuje současný stav jako výchozí.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Nic nemažeme, protože tabulka existovala už před EF migracemi.
        }
    }
}