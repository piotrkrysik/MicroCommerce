using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Kolekcja produktów w danej kategorii
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
