namespace Stockify.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stockify.Models;

public class TransactionRepository : ITransactionRepository
{
    private readonly StockifyContext _context;

    public TransactionRepository(StockifyContext context)
    {
        _context = context;
    }
    public Transaction Get(int id)
    {
        return _context.Transactions.Include(p => p.Product).FirstOrDefault(p => p.Id == id);
    }
    public List<Transaction> GetAll(HttpContext httpContext)
    {
        var user = httpContext.User;
        var tenantIdClaim = user.FindFirst("TenantId");
        var tenantId = int.Parse(tenantIdClaim.Value);
        var transactions = _context.Transactions.Include(p => p.Product).Where(p => p.Product.Inventory.TenantId == tenantId).ToList();
        return transactions;
    }
    public List<Transaction> GetTransactionsByProductId(int id)
    {
        var transactions = _context.Transactions.Include(p => p.Product).Where(t => t.ProductId == id).ToList();
        return transactions;
    }
    public List<Transaction> GetTransactionsByInventoryId(int id)
    {
        var transactions = _context.Transactions.Include(p => p.Product).Where(t => t.Product.InventoryId == id).ToList();
        return transactions;
    }
    public void Add(Transaction Transaction)
    {
        _context.Transactions.Add(Transaction);
        SaveChanges();
    }
    public void Update(Transaction Transaction)
    {

    }
    public void Delete(int id)
    {
        var Transaction = _context.Transactions.Find(id);
        _context.Remove(Transaction);
        SaveChanges();
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
