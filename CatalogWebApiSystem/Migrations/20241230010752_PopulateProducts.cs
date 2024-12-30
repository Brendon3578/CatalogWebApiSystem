using Microsoft.EntityFrameworkCore.Migrations;
using System.Globalization;

#nullable disable

namespace CatalogWebApiSystem.Migrations
{
    /// <inheritdoc />
    public partial class PopulateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var products = new List<(string Name, string Description, double Price, string ImageUrl, int CategoryId)>
            {
                ("Coca-Cola Diet", "Refrigerante de Cola 350 ml", 5.45, "https://picsum.photos/300/400", 1),
                ("Lanche de Atum", "Lanche de Atum com maionese", 8.50, "https://picsum.photos/300/400", 2),
                ("Pudim 100 g", "Pudim de leite condensado 100g", 6.75, "https://picsum.photos/300/400", 3),
                ("Pizza Margherita", "Pizza com molho de tomate, queijo e manjericão", 25.90, "https://picsum.photos/300/400", 4),
                ("Salada Caesar", "Salada com alface, frango, croutons e molho Caesar", 15.00, "https://picsum.photos/300/400", 5)
            };
            foreach (var (Name, Description, Price, ImageUrl, CategoryId) in products)
                migrationBuilder.Sql($@"INSERT INTO Products (Name, Description, Price, Stock, ImageUrl, CreatedOn, CategoryId)
                    VALUES ('{Name}', '{Description}', {Price.ToString(CultureInfo.InvariantCulture)}, 50, '{ImageUrl}', NOW(), {CategoryId})");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Products");
        }
    }
}
