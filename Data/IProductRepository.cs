using Stockify.Models;

namespace Stockify.Data;

public interface IProductRepository{
    List<Product> GetAll();
    Product Get(int id);
    void Add (Product Product);
    void Update (Product Product);
    void Delete (int id);
}
