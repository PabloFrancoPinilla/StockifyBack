namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class TenantCreateDto {
    
    public string Name { get; set;}
    public string Password { get; set;}

    public string Contact{get;set;}
    public TenantCreateDto(){}

}