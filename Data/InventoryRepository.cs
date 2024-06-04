namespace Stockify.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stockify.Models;

public class InventoryRepository : IInventoryRepository
{
    private readonly StockifyContext _context;

    public InventoryRepository(StockifyContext context)
    {
        _context = context;
    }
    public InventoryDto Get(int id)
    {
        var inventory = _context.Inventories.Include(p => p.Products).ThenInclude(p => p.ProductCategories)
                 .ThenInclude(pc => pc.Category).FirstOrDefault(p => p.Id == id); ;
        if (inventory == null)
        {

            return null;
        }

        var inventoryDto = new InventoryDto
        {
            Id = inventory.Id,
            Name = inventory.Name,
            CreationDate = inventory.CreationDate,
            Products = inventory.Products.Select(p => new ProductDto
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
            }).ToList()
        };

        return inventoryDto;
    }
    public List<InventoryDto> GetAll(HttpContext httpContext)
    {
        var user = httpContext.User;
        var tenantIdClaim = user.FindFirst("TenantId");
        if (tenantIdClaim == null)
        {
            // Manejar el caso en el que no se puede encontrar el TenantId en el token JWT
            return new List<InventoryDto>();
        }
        int tenantId = Convert.ToInt32(tenantIdClaim.Value);
        var inventories = _context.Inventories.Include(p => p.Products).ThenInclude(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category).Where(p => p.TenantId == tenantId).ToList();
        var inventoriesDto = inventories.Select(p => new InventoryDto
        {
            Id = p.Id,
            Name = p.Name,
            CreationDate = p.CreationDate,
            Image = p.Image,
            Products = p.Products.Select(p => new ProductDto
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
            }).ToList()
        }).ToList();
        return inventoriesDto;

    }
    public void Add(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        SaveChanges();
    }
    public void Update(Inventory inventory)
    {

    }
    public void Delete(int id)
    {
        var inventory = _context.Inventories
       .Include(i => i.Products)
       .ThenInclude(p => p.ProductCategories)
       .Include(i => i.Categories)
       .FirstOrDefault(i => i.Id == id);

        if (inventory != null)
        {
            // Eliminar las relaciones de ProductCategory
            foreach (var product in inventory.Products)
            {
                _context.ProductsCategory.RemoveRange(product.ProductCategories);
            }

            // Eliminar los productos
            _context.Products.RemoveRange(inventory.Products);

            // Eliminar las categorías
            _context.Categories.RemoveRange(inventory.Categories);

            // Eliminar el inventario
            _context.Inventories.Remove(inventory);

            _context.SaveChanges();
        }
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
