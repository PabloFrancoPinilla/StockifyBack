namespace Stockify.Business;
using Stockify.Models;

public interface ITransactionService
{
    void Add(Transaction Transaction);
    List<Transaction> GetTransactionsByInventoryId(int id);
    List<Transaction> GetTransactionsByProductId(int id);
    void Delete(int id);
    void Update(Transaction Transaction);
    Transaction Get(int id);
    List<Transaction> GetAll();
}