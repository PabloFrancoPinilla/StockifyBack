namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class InventoryDto
{
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string CreationDate { get; set; }

    public string Color { get; set; }
    public string Image { get; set; }
    public List<ProductDto> Products { get; set; }
    public InventoryDto() { }

}
