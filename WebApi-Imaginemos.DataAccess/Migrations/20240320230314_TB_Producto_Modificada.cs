using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_Imaginemos.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TB_Producto_Modificada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precion",
                table: "Producto",
                newName: "Precio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Producto",
                newName: "Precion");
        }
    }
}
