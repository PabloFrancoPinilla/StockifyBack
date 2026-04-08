namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class CategoryCreateDto{
    public string Name { get; set;}
    public int InventoryId { get; set;}
    public CategoryCreateDto() { }

}