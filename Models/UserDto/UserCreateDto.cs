namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class UserCreateDto{
    [Required]
    public string Name { get; set;}
    [Required]
    public string LastName {get; set;}
    [Required]
    [EmailAddress]
    public string Email {get;set;}
    [Required]
    public string Password {get; set;}
    public string TenantName {get; set;}    

   
    public UserCreateDto(){}




}