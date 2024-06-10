namespace Stockify.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stockify.Models;
using Microsoft.Extensions.Logging;

public class TransactionRepository : ITransactionRepository
{
    private readonly StockifyContext _context;
    private readonly ILogger<TransactionRepository> _logger;

    public TransactionRepository(StockifyContext context, ILogger<TransactionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Transaction Get(int id)
    {
        try
        {
            return _context.Transactions.Include(p => p.Product).FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transaction with id {Id}", id);
            throw;
        }
    }

    public List<Transaction> GetAll(HttpContext httpContext)
    {
        try
        {
            var user = httpContext.User;
            var tenantIdClaim = user.FindFirst("TenantId");
            var tenantId = int.Parse(tenantIdClaim.Value);
            var transactions = _context.Transactions.Include(p => p.Product)
                .Where(p => p.Product.Inventory.TenantId == tenantId).ToList();
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all transactions");
            throw;
        }
    }

    public List<Transaction> GetTransactionsByProductId(int id)
    {
        try
        {
            var transactions = _context.Transactions.Include(p => p.Product)
                .Where(t => t.ProductId == id).ToList();
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transactions by product id {Id}", id);
            throw;
        }
    }

    public List<Transaction> GetTransactionsByInventoryId(int id)
    {
        try
        {
            var transactions = _context.Transactions.Include(p => p.Product)
                .Where(t => t.Product.InventoryId == id).ToList();
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transactions by inventory id {Id}", id);
            throw;
        }
    }

    public void Add(Transaction transaction)
    {
        try
        {
            _context.Transactions.Add(transaction);
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding transaction");
            throw;
        }
    }

    public void Update(Transaction transaction)
    {
        try
        {
         
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transaction");
            throw;
        }
    }

    public void Delete(int id)
    {
        try
        {
            var transaction = _context.Transactions.Find(id);
            _context.Remove(transaction);
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting transaction with id {Id}", id);
            throw;
        }
    }

    public void SaveChanges()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving changes");
            throw;
        }
    }
}
