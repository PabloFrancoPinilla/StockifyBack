using Stockify.Models;

namespace Stockify.Data;

public interface IRoleRepository{
    List<Role> GetAll();
    Role Get(int id);
    void Add (Role Role);
    void Update (Role Role);
    void Delete (int id);
}
