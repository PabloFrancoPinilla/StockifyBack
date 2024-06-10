namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class TenantDto{
    [Required]
    public int Id { get; set; }
   
    public string Name { get; set;}

    public string Contact{get;set;}
    public string Role{get;set;}
    public string Service{get;set;}
    public TenantDto(){}

}