namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class InventoryCreateDto
{
    public string Name { get; set; }
    public string CreationDate { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public string Image { get; set; }
    public string Location { get; set; }

    public int TenantId { get; set; }
    public InventoryCreateDto() { }

}
