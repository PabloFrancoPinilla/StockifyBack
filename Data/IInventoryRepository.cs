using Microsoft.AspNetCore.Http;
using Stockify.Models;

namespace Stockify.Data;

public interface IInventoryRepository{
    List<InventoryDto> GetAll(HttpContext httpContext);
    InventoryDto Get(int id);
   void Add(HttpContext httpContext, Inventory inventory);
    void Update (Inventory inventory);
    void Delete (int id);
}
