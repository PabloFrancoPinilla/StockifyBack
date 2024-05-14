using Stockify.Models;

namespace Stockify.Data;

public interface IUserRepository{
    List<User> GetAll();
    User Get(int id);
    void Add (User User);
    void Update (User User);
    void Delete (int id);
}
