using Microsoft.AspNetCore.Http;
using Stockify.Models;

namespace Stockify.Data;

public interface IProductRepository
{
    List<Product> GetAll();
    ProductDto Get(int id);
    List<ProductDto> GetProductsByInventoryId(int id);
    ProductDto Add(ProductCreateDto productCreateDto, HttpContext httpContext);
    void Update(ProductUpdateDto updatedProductDto, HttpContext httpContext);
    void Delete(int id);
}
