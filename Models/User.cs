namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class User{
    [Required]
    public int Id { get; set;}
    public string Name { get; set;}
    public string LastName {get; set;}
    public string Email {get;set;}
    public string Password {get; set;}

    public int RoleId { get; set;}
    public Role Role {get; set;}
    public int TenantId { get; set;}
    public Tenant Tenant{get; set;}
    public User(){}




}