namespace Stockify.Business;
using Stockify.Models;

public interface IUserService {
    void Add(User User);
    void Delete(int id);
    void Update(User User);
    User Get (int id);
    List<User> GetAll();
}