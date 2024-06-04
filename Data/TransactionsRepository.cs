namespace Stockify.Data;

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
    public List<Transaction> GetAll()
    {
        return _context.Transactions.Include(p => p.Product).ToList();
    }
    public void Add(Transaction Transaction)
    {
        _context.Transactions.Add(Transaction);
        SaveChanges();
    }
    public void Update (Transaction Transaction){
        
    }
    public void Delete (int id){
        var Transaction = _context.Transactions.Find(id);
        _context.Remove(Transaction);
        SaveChanges(); 
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
