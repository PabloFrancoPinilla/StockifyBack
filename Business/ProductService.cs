namespace Stockify.Business;

using Microsoft.AspNetCore.Http;
using Stockify.Data;
using Stockify.Models;
public class ProductService : IProductService
{
private readonly IProductRepository _ProductRepository;
public ProductService(IProductRepository ProductRepository){
    _ProductRepository = ProductRepository;
}
public ProductDto Get(int id) => _ProductRepository.Get(id);
public List<ProductDto> GetProductsByInventoryId(int id) => _ProductRepository.GetProductsByInventoryId(id);
public void Update(ProductUpdateDto updatedProductDto, HttpContext httpContext) => _ProductRepository.Update(updatedProductDto,httpContext);
public void Delete(int id)=>_ProductRepository.Delete(id);
public List<Product> GetAll () => _ProductRepository.GetAll();
public ProductDto Add(ProductCreateDto productCreateDto, HttpContext httpContext) => _ProductRepository.Add(productCreateDto, httpContext);

}
