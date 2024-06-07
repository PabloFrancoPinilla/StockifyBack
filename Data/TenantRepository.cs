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
    public TenantDto GetTenantFromCredentials(LoginRequest loginRequest)
    {
        var tenantOut = _context.Tenants.FirstOrDefault(t =>
            t.Name.ToLower() == loginRequest.Username.ToLower() &&
            t.Password == loginRequest.Password);

        if (tenantOut == null)
        {
            return null;
        }

        var tenantDto = new TenantDto
        {
            Id = tenantOut.Id,
            Name = tenantOut.Name,
            Contact = tenantOut.Contact,
            Role = tenantOut.Role,
            Service = tenantOut.Service
        };

        return tenantDto;
    }
    public void Add(Tenant Tenant)
    {
        _context.Tenants.Add(Tenant);
        SaveChanges();
    }
    public void Update(Tenant Tenant)
    {

    }
    public void Delete(int id)
    {
        var Tenant = _context.Tenants.Find(id);
        _context.Remove(Tenant);
        SaveChanges();
    }
    public string Login(string username, string password)
    {
        var tenant = _context.Tenants.FirstOrDefault(p => p.Name == username);
        if (tenant == null)
        {
            return "Tenant not found";
        }
        else if (tenant.Password == password)
        {
            return "Tenant found";
        }
        else
        {
            return "Invalid Credentials";
        }
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
