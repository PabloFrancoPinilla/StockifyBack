namespace Stockify.Business;
using Stockify.Data;
using Stockify.Models;
public class RoleService : IRoleService
{
private readonly IRoleRepository _RoleRepository;
public RoleService(IRoleRepository RoleRepository){
    _RoleRepository = RoleRepository;
}
public Role Get(int id) => _RoleRepository.Get(id);
public void Update(Role Role) => _RoleRepository.Update(Role);
public void Delete(int id)=>_RoleRepository.Delete(id);
public List<Role> GetAll () => _RoleRepository.GetAll();
public void Add (Role Role) => _RoleRepository.Add(Role);

}
