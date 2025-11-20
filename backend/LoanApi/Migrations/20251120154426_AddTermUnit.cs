using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTermUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TermUnit",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermUnit",
                table: "Loans");
        }
    }
}
