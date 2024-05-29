namespace Stockify.Business;
using Stockify.Models;

public interface IProductService {
    ProductDto Add(ProductCreateDto product);
    void Delete(int id);
    void Update(Product Product);
    ProductDto Get(int id);
    List<Product> GetAll();
}