namespace Stockify.Data;

using Microsoft.EntityFrameworkCore;
using Stockify.Models;

public class ProductRepository : IProductRepository
{
    private readonly StockifyContext _context;

    public ProductRepository(StockifyContext context)
    {
        _context = context;
    }
    public ProductDto Get(int id)
    {
        var product = _context.Products
        .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return null;
        }

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
            Categories = product.ProductCategories.Select(pc => new CategoryDto
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name
            }).ToList()
        };
    }
    public List<Product> GetAll()
    {
        return _context.Products.ToList();
    }
    public ProductDto Add(ProductCreateDto productCreateDto)
    {
        var product = new Product
        {
            Name = productCreateDto.Name,
            Description = productCreateDto.Description,
            Price = productCreateDto.Price,
            Quantity = productCreateDto.Quantity,
            InventoryId = productCreateDto.InventoryId, 
            ProductCategories = productCreateDto.CategoriesId.Select(id => new ProductCategory { CategoryId = id }).ToList()
        };

        _context.Products.Add(product);
        _context.SaveChanges();
        _context.Entry(product).Collection(p => p.ProductCategories).Load();
        foreach (var productCategory in product.ProductCategories)
        {
            _context.Entry(productCategory).Reference(pc => pc.Category).Load();
        }
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
            Categories = product.ProductCategories.Select(pc => new CategoryDto
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name
            }).ToList()
        };
    }
    public void Update(Product Product)
    {

    }
    public void Delete(int id)
    {
        var Product = _context.Products.Find(id);
        _context.Remove(Product);
        SaveChanges();
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
