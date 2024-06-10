using System.Collections.Generic;

namespace Stockify.Models;

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }

