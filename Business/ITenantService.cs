namespace Stockify.Business;
using Stockify.Models;

public interface ITenantService {
    void Add(Tenant tenant);
    string Login (string UserName, string Password);
    void Delete(int id);
    void Update(Tenant Tenant);
    Tenant Get (int id);
    List<Tenant> GetAll();
    TenantDto GetTenantFromCredentials(LoginRequest loginRequest);
}