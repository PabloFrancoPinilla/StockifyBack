using Stockify.Models;

namespace Stockify.Data;

public interface IInventoryRepository{
    List<Inventory> GetAll();
    Inventory Get(int id);
    void Add (Inventory inventory);
    void Update (Inventory inventory);
    void Delete (int id);
}
