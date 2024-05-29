using Stockify.Models;

namespace Stockify.Data;

public interface IProductRepository
{
    List<Product> GetAll();
    ProductDto Get(int id);
    ProductDto Add(ProductCreateDto productCreateDto);
    void Update(Product Product);
    void Delete(int id);
}
