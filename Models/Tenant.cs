namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Tenant{
    [Required]
    public int Id { get; set;}
    public string Name { get; set;}

    public string Contact{get;set;}

    public int InventoryId { get; set;}
    public Inventory Inventory { get; set;}
    public Tenant(){}

}