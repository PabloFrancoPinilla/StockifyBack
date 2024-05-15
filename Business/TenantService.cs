namespace Stockify.Business;
using Stockify.Data;
using Stockify.Models;
public class TenantService : ITenantService
{
private readonly ITenantRepository _TenantRepository;
public TenantService(ITenantRepository TenantRepository){
    _TenantRepository = TenantRepository;
}
public Tenant Get(int id) => _TenantRepository.Get(id);
public void Update(Tenant Tenant) => _TenantRepository.Update(Tenant);
public void Delete(int id)=>_TenantRepository.Delete(id);
public List<Tenant> GetAll () => _TenantRepository.GetAll();
public void Add (Tenant Tenant) => _TenantRepository.Add(Tenant);
public string Login (string username, string password)=> _TenantRepository.Login(username, password);

}
