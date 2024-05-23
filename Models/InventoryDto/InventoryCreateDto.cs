namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class InventoryCreateDto
{
    public string Name { get; set;}
    public string CreationDate {get;set;}
    public int TenantId { get; set;}
    public InventoryCreateDto(){}

}
