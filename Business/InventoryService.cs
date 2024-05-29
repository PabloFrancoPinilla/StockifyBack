namespace Stockify.Business;

using Microsoft.AspNetCore.Http;
using Stockify.Data;
using Stockify.Models;
public class InventoryService : IInventoryService
{
private readonly IInventoryRepository _inventoryRepository;
public InventoryService(IInventoryRepository inventoryRepository){
    _inventoryRepository = inventoryRepository;
}
public InventoryDto Get(int id) => _inventoryRepository.Get(id);
public void Update(Inventory inventory) => _inventoryRepository.Update(inventory);
public void Delete(int id)=>_inventoryRepository.Delete(id);
public  List<InventoryDto> GetAll (HttpContext httpContext) => _inventoryRepository.GetAll(httpContext);
public void Add (Inventory inventory) => _inventoryRepository.Add(inventory);

}
