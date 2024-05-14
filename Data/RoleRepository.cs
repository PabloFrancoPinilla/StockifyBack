namespace Stockify.Data;
using Stockify.Models;

public class RoleRepository : IRoleRepository
{
    private readonly StockifyContext _context;

    public RoleRepository(StockifyContext context)
    {
        _context = context;
    }
    public Role Get(int id)
    {
        return _context.Roles.FirstOrDefault(p => p.Id == id);
    }
    public List<Role> GetAll()
    {
        return _context.Roles.ToList();
    }
    public void Add(Role Role)
    {
        _context.Roles.Add(Role);
        SaveChanges();
    }
    public void Update (Role Role){
        
    }
    public void Delete (int id){
        var Role = _context.Roles.Find(id);
        _context.Remove(Role);
        SaveChanges(); 
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
