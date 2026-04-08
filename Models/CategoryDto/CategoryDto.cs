namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class CategoryDto{
    [Required]
    public int Id { get; set;}
    public string Name { get; set;}
    public CategoryDto() { }

}