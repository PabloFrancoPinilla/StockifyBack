namespace Stockify.Data;
using Stockify.Models;

public class ProductRepository : IProductRepository
{
    private readonly StockifyContext _context;

    public ProductRepository(StockifyContext context)
    {
        _context = context;
    }
    public Product Get(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }
    public List<Product> GetAll()
    {
        return _context.Products.ToList();
    }
    public void Add(Product Product)
    {
        _context.Products.Add(Product);
        SaveChanges();
    }
    public void Update (Product Product){
        
    }
    public void Delete (int id){
        var Product = _context.Products.Find(id);
        _context.Remove(Product);
        SaveChanges(); 
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
