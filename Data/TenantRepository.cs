namespace Stockify.Data;
using Stockify.Models;

public class TenantRepository : ITenantRepository
{
    private readonly StockifyContext _context;

    public TenantRepository(StockifyContext context)
    {
        _context = context;
    }
    public Tenant Get(int id)
    {
        return _context.Tenants.FirstOrDefault(p => p.Id == id);
    }
    public List<Tenant> GetAll()
    {
        return _context.Tenants.ToList();
    }
    public void Add(Tenant Tenant)
    {
        _context.Tenants.Add(Tenant);
        SaveChanges();
    }
    public void Update (Tenant Tenant){
        
    }
    public void Delete (int id){
        var Tenant = _context.Tenants.Find(id);
        _context.Remove(Tenant);
        SaveChanges(); 
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
