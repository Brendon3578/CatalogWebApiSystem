using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogWebApiSystem.Migrations
{
    /// <inheritdoc />
    public partial class PopulateCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var categories = new List<(string Name, string ImageUrl)>
            {
                ("Bebidas", "https://picsum.photos/200/300"),
                ("Lanches", "https://picsum.photos/200/300"),
                ("Sobremesas", "https://picsum.photos/200/300"),
                ("Massas", "https://picsum.photos/200/300"),
                ("Saladas", "https://picsum.photos/200/300")
            };

            foreach (var (Name, ImageUrl) in categories)
                migrationBuilder.Sql($@"INSERT INTO Categories (Name, ImageUrl) VALUES ('{Name}', '{ImageUrl}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories");
        }
    }
}
