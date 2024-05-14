namespace Stockify.Business;
using Stockify.Models;

public interface IRoleService {
    void Add(Role Role);
    void Delete(int id);
    void Update(Role Role);
    Role Get (int id);
    List<Role> GetAll();
}