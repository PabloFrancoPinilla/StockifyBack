namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Category{
    [Required]
    public int Id { get; set;}
    public string Name { get; set;}
    public List<ProductCategory> ProductCategories { get; set;}
    public Category() { }

}