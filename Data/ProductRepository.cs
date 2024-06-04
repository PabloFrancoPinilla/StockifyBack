namespace Stockify.Data;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
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
    public List<ProductDto> GetProductsByInventoryId(int id)
    {

        var products = _context.Products
        .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Where(pc => pc.InventoryId == id);

        if (products == null)
        {
            return null;
        }

        var productdto = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Quantity = p.Quantity,
            Categories = p.ProductCategories.Select(pc => new CategoryDto
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name
            }).ToList()
        }).ToList();

        return productdto;
    }

    public List<Product> GetAll()
    {
        return _context.Products.ToList();
    }
    public ProductDto Add(ProductCreateDto productCreateDto, HttpContext httpContext)
    {
        var user = httpContext.User;
        var makerNameClaim = user.FindFirst(ClaimTypes.Name);
        string makerName = makerNameClaim != null ? makerNameClaim.Value : "Unknown";

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

        var transaction = new Transaction
        {
            Type = "Add",
            Date = DateTime.UtcNow,
            ProductId = product.Id,
            MakerName = makerName,
            Quantity = product.Quantity
        };

        _context.Transactions.Add(transaction);
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

    public void Update(ProductUpdateDto updatedProductDto, HttpContext httpContext)
{
    var user = httpContext.User;
    var makerNameClaim = user.FindFirst(ClaimTypes.Name);
    string makerName = makerNameClaim != null ? makerNameClaim.Value : "Unknown";

    var existingProduct = _context.Products
        .Include(p => p.ProductCategories)
        .FirstOrDefault(p => p.Id == updatedProductDto.Id);

    if (existingProduct != null)
    {
        int quantityDifference = updatedProductDto.Quantity - existingProduct.Quantity;

        existingProduct.Name = updatedProductDto.Name;
        existingProduct.Description = updatedProductDto.Description;
        existingProduct.Price = updatedProductDto.Price;
        existingProduct.Quantity = updatedProductDto.Quantity;

        var existingCategoryIds = existingProduct.ProductCategories.Select(pc => pc.CategoryId).ToList();
        var updatedCategories = _context.Categories.Where(c => updatedProductDto.CategoryNames.Contains(c.Name)).ToList();
        var updatedCategoryIds = updatedCategories.Select(c => c.Id).ToList();

        var categoriesToRemove = existingProduct.ProductCategories
            .Where(pc => !updatedCategoryIds.Contains(pc.CategoryId))
            .ToList();
        _context.ProductsCategory.RemoveRange(categoriesToRemove);

        var categoriesToAdd = updatedCategoryIds
            .Where(id => !existingCategoryIds.Contains(id))
            .Select(id => new ProductCategory { ProductId = updatedProductDto.Id, CategoryId = id })
            .ToList();
        _context.ProductsCategory.AddRange(categoriesToAdd);

        _context.SaveChanges();

        if (quantityDifference != 0)
        {
            var transaction = new Transaction
            {
                Type = quantityDifference > 0 ? "Add" : "Remove",
                Date = DateTime.UtcNow,
                ProductId = existingProduct.Id,
                MakerName = makerName,
                Quantity = Math.Abs(quantityDifference)
            };

            _context.Transactions.Add(transaction);
        }

        _context.SaveChanges();

        _context.Entry(existingProduct).Collection(p => p.ProductCategories).Load();
        foreach (var productCategory in existingProduct.ProductCategories)
        {
            _context.Entry(productCategory).Reference(pc => pc.Category).Load();
        }
    }
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
