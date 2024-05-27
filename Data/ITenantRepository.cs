using Stockify.Models;

namespace Stockify.Data;

public interface ITenantRepository{
    List<Tenant> GetAll();
    Tenant Get(int id);
    void Add(Tenant tenant);
    void Update (Tenant Tenant);
    void Delete (int id);
    string Login (string username, string password);
    TenantDto GetTenantFromCredentials(LoginRequest loginRequest);
}
