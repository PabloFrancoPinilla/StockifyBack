namespace Stockify.Business;

using Microsoft.AspNetCore.Http;
using Stockify.Models;

public interface IProductService {
    ProductDto Add(ProductCreateDto product, HttpContext httpContext);
    void Delete(int id);
    void Update(ProductUpdateDto updatedProductDto, HttpContext httpContext);
    ProductDto Get(int id);
    List<ProductDto> GetProductsByInventoryId(int id);
    List<ProductDto> GetProductsByTenantId( HttpContext httpContext);
    List<Product> GetAll();
}