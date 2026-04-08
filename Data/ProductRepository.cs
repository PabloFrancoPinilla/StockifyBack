namespace Stockify.Data;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stockify.Models;
using Microsoft.Extensions.Logging;

public class ProductRepository : IProductRepository
{
    private readonly StockifyContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(StockifyContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    private string GetTenantService(HttpContext httpContext)
    {
        try
        {
            var user = httpContext.User;
            var serviceTypeClaim = user.FindFirst("Service"); // Suponiendo que el token incluye este claim
            return serviceTypeClaim?.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant service from HttpContext");
            throw;
        }
    }

    private int GetTenantId(HttpContext httpContext)
    {
        try
        {
            var user = httpContext.User;
            var tenantIdClaim = user.FindFirst("TenantId");
            return int.Parse(tenantIdClaim.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant ID from HttpContext");
            throw;
        }
    }

    private bool CanCreateMoreUsers(HttpContext httpContext)
    {
        try
        {
            string serviceType = GetTenantService(httpContext);
            int tenantId = GetTenantId(httpContext);
            int userCount = _context.Users.Count(i => i.TenantId == tenantId);

            return serviceType switch
            {
                Services.Free => userCount < 1,
                Services.Basic => userCount < 5,
                Services.Premium => true,
                _ => false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if more users can be created");
            throw;
        }
    }

    public ProductDto Get(int id)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with id {Id}", id);
            throw;
        }
    }

    public List<ProductDto> GetProductsByInventoryId(int id)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products by inventory id {Id}", id);
            throw;
        }
    }

    public List<ProductDto> GetProductsByTenantId(HttpContext httpContext)
    {
        try
        {
            var user = httpContext.User;
            var tenantIdClaim = user.FindFirst("TenantId");
            if (tenantIdClaim == null)
            {
                // Manejar el caso en el que no se puede encontrar el TenantId en el token JWT
                return new List<ProductDto>();
            }
            int tenantId = Convert.ToInt32(tenantIdClaim.Value);

            var products = _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(pc => pc.Inventory.TenantId == tenantId);

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products by tenant id");
            throw;
        }
    }

    public List<Product> GetAll()
    {
        try
        {
            return _context.Products.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            throw;
        }
    }

    public ProductDto Add(ProductCreateDto productCreateDto, HttpContext httpContext)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product");
            throw;
        }
    }

    public void Update(ProductUpdateDto updatedProductDto, HttpContext httpContext)
    {
        try
        {
            var user = httpContext.User;
            var makerNameClaim = user.FindFirst(ClaimTypes.Name);
            string makerName = makerNameClaim != null ? makerNameClaim.Value : "Unknown";

            var existingProduct = _context.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefault(p => p.Id == updatedProductDto.Id);

            if (existingProduct != null)
            {
                int quantityChange = updatedProductDto.Quantity;

                existingProduct.Name = updatedProductDto.Name;
                existingProduct.Description = updatedProductDto.Description;
                existingProduct.Price = updatedProductDto.Price;
                existingProduct.Quantity += quantityChange; // Ajustar la cantidad existente

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

                if (quantityChange != 0)
                {
                    var transaction = new Transaction
                    {
                        Type = quantityChange > 0 ? "Add" : "Remove",
                        Date = DateTime.UtcNow,
                        ProductId = existingProduct.Id,
                        MakerName = makerName,
                        Quantity = quantityChange,
                        Total = existingProduct.Quantity,
                        Price = existingProduct.Price
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with id {Id}", updatedProductDto.Id);
            throw;
        }
    }

    public void Delete(int id)
    {
        try
        {
            var product = _context.Products.Find(id);
            _context.Remove(product);
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with id {Id}", id);
            throw;
        }
    }

    public void SaveChanges()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving changes");
            throw;
        }
    }
}
