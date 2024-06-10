namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class UserDto{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set;}
    [Required]
    public string LastName {get; set;}
    [Required]
    [EmailAddress]
    public string Email {get;set;}
    [Required]
    public string TenantName {get; set;}  
     public int TenantId {get; set;} 
    public string Role {get; set;}  

   
    public UserDto(){}




}