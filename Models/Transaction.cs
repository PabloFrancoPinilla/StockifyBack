namespace Stockify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Transaction
{
    [Required]
    public int Id { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string MakerName { get; set; }
    public int Quantity { get; set; }
    public int Total { get; set; }

    public decimal Price { get; set; }

    public Transaction() { }


}