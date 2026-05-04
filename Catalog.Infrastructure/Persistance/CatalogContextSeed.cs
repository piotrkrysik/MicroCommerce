using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Persistence
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext catalogContext)
        {
            if (!catalogContext.Categories.Any())
            {
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Elektronika"
                };

                catalogContext.Categories.Add(category);
                await catalogContext.SaveChangesAsync();

                if (!catalogContext.Products.Any())
                {
                    catalogContext.Products.AddRange(new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Smartfon X",
                    Summary = "Świetny telefon",
                    Price = 2999.00m,
                    CategoryId = category.Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Laptop Y",
                    Summary = "Szybki komputer",
                    Price = 4500.00m,
                    CategoryId = category.Id
                }
            });

                    await catalogContext.SaveChangesAsync();
                }
            }
        }
    }
}
