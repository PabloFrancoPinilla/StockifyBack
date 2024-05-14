namespace Stockify.Business;
using Stockify.Models;

public interface IProductService {
    void Add(Product Product);
    void Delete(int id);
    void Update(Product Product);
    Product Get (int id);
    List<Product> GetAll();
}