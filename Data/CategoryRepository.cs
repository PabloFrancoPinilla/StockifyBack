namespace Stockify.Data;
using Stockify.Models;

public class CategoryRepository : ICategoryRepository
{
    private readonly StockifyContext _context;

    public CategoryRepository(StockifyContext context)
    {
        _context = context;
    }
    public Category Get(int id)
    {
        return _context.Categories.FirstOrDefault(p => p.Id == id);
    }
    public List<Category> GetAll()
    {
        return _context.Categories.ToList();
    }
    public void Add(Category Category)
    {
        _context.Categories.Add(Category);
        SaveChanges();
    }
    public void Update (Category Category){
        
    }
    public void Delete (int id){
        var Category = _context.Categories.Find(id);
        _context.Remove(Category);
        SaveChanges(); 
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
