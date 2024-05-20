namespace Stockify.Data;
using Stockify.Models;

public class InventoryRepository : IInventoryRepository
{
    private readonly StockifyContext _context;

    public InventoryRepository(StockifyContext context)
    {
        _context = context;
    }
    public Inventory Get(int id)
    {
        return _context.Inventories.FirstOrDefault(p => p.Id == id);
    }
    public List<Inventory> GetAll()
    {
        return _context.Inventories.ToList();
    }
    public void Add(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        SaveChanges();
    }
    public void Update (Inventory inventory){
        
    }
    public void Delete (int id){
        var inventory = _context.Inventories.Find(id);
        _context.Remove(inventory);
        SaveChanges(); 
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
