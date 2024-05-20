namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Inventory
{
    [Required]
    public int Id { get; set;}
    public string Name { get; set;}
    public string CreationDate {get;set;}
    public int TenantId { get; set;}
    public Tenant Tenant { get; set;}

    public List<Product> Products { get; set;}
    public Inventory(){}

}
