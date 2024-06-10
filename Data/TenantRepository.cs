namespace Stockify.Data;
using Stockify.Models;
using Microsoft.Extensions.Logging; 

public class TenantRepository : ITenantRepository
{
    private readonly StockifyContext _context;
    private readonly ILogger<TenantRepository> _logger;

    public TenantRepository(StockifyContext context, ILogger<TenantRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Tenant Get(int id)
    {
        try
        {
            return _context.Tenants.FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant with id {Id}", id);
            throw;
        }
    }

    public List<Tenant> GetAll()
    {
        try
        {
            return _context.Tenants.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all tenants");
            throw;
        }
    }

    public TenantDto GetTenantFromCredentials(LoginRequest loginRequest)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant from credentials");
            throw;
        }
    }

    public void Add(Tenant tenant)
    {
        try
        {
            _context.Tenants.Add(tenant);
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding tenant");
            throw;
        }
    }

    public void Update(Tenant tenant)
    {
        try
        {
        
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tenant");
            throw;
        }
    }

    public void Delete(int id)
    {
        try
        {
            var tenant = _context.Tenants.Find(id);
            _context.Remove(tenant);
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tenant with id {Id}", id);
            throw;
        }
    }

    public string Login(string username, string password)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for username {Username}", username);
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
