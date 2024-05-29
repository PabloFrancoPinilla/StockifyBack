namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class ProductCreateDto{
   
    public string Name { get; set;}
    public string Description {get;set;}
    public decimal Price { get; set;}
    public int Quantity { get; set;}
    public int InventoryId { get; set;}
    public List<int> CategoriesId { get; set;}
    public ProductCreateDto(){}

}