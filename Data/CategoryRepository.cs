namespace Stockify.Data;
using Stockify.Models;
using Microsoft.Extensions.Logging;

public class CategoryRepository : ICategoryRepository
{
    private readonly StockifyContext _context;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(StockifyContext context, ILogger<CategoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Category Get(int id)
    {
        try
        {
            return _context.Categories.FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category with id {Id}", id);
            throw;
        }
    }

    public List<Category> GetAll()
    {
        try
        {
            return _context.Categories.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all categories");
            throw;
        }
    }

    public List<CategoryDto> GetCategoriesByInventoryId(int id)
    {
        try
        {
            var categories = _context.Categories.Where(p => p.InventoryId == id);
            var categoriesdto = categories.Select(p => new CategoryDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return categoriesdto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories by inventory id {Id}", id);
            throw;
        }
    }

    public void Add(Category Category)
    {
        try
        {
            _context.Categories.Add(Category);
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding category");
            throw;
        }
    }

    public void Update(Category Category)
    {
        // Implementation for updating category
    }

    public void Delete(int id)
    {
        try
        {
            var Category = _context.Categories.Find(id);
            _context.Remove(Category);
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category with id {Id}", id);
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
