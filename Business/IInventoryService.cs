namespace Stockify.Business;

using Microsoft.AspNetCore.Http;
using Stockify.Models;

public interface IInventoryService {
    void Add(Inventory inventory);
    void Delete(int id);
    void Update(Inventory inventory);
    InventoryDto Get(int id);
     List<InventoryDto> GetAll(HttpContext httpContext);
}