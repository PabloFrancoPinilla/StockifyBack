namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Product{
    [Required]
    public int Id { get; set;}
    public string Name { get; set;}
    public string Description {get;set;}
    public decimal Price { get; set;}
    public int Quantity { get; set;}
    public List<ProductCategory> ProductCategories { get; set;}
    public int InventoryId { get; set;}
    public Inventory Inventory { get; set;}
    public Product(){}

}