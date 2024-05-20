namespace Stockify.Business;
using Stockify.Data;
using Stockify.Models;
public class ProductService : IProductService
{
private readonly IProductRepository _ProductRepository;
public ProductService(IProductRepository ProductRepository){
    _ProductRepository = ProductRepository;
}
public Product Get(int id) => _ProductRepository.Get(id);
public void Update(Product Product) => _ProductRepository.Update(Product);
public void Delete(int id)=>_ProductRepository.Delete(id);
public List<Product> GetAll () => _ProductRepository.GetAll();
public void Add (Product Product) => _ProductRepository.Add(Product);

}
