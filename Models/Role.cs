namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Role{
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public Role(){}

    

}
