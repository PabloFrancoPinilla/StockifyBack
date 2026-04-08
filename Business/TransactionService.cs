namespace Stockify.Business;

using Microsoft.AspNetCore.Http;
using Stockify.Data;
using Stockify.Models;
public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _TransactionRepository;
    public TransactionService(ITransactionRepository TransactionRepository)
    {
        _TransactionRepository = TransactionRepository;
    }
    public Transaction Get(int id) => _TransactionRepository.Get(id);
    public List<Transaction> GetTransactionsByInventoryId(int id) => _TransactionRepository.GetTransactionsByInventoryId(id);
    public List<Transaction> GetTransactionsByProductId(int id) => _TransactionRepository.GetTransactionsByProductId(id);
    public void Update(Transaction Transaction) => _TransactionRepository.Update(Transaction);
    public void Delete(int id) => _TransactionRepository.Delete(id);
    public List<Transaction> GetAll(HttpContext httpContext) => _TransactionRepository.GetAll(httpContext);
    public void Add(Transaction Transaction) => _TransactionRepository.Add(Transaction);

}
