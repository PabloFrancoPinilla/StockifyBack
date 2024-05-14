namespace Stockify.Business;
using Stockify.Models;

public interface IInventoryService {
    void Add(Inventory inventory);
    void Delete(int id);
    void Update(Inventory inventory);
    Inventory Get (int id);
    List<Inventory> GetAll();
}