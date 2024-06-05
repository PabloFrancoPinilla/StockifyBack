using Stockify.Models;

namespace Stockify.Data;

public interface ITransactionRepository
{
    List<Transaction> GetAll();
    List<Transaction> GetTransactionsByInventoryId(int id);
    List<Transaction> GetTransactionsByProductId(int id);
    Transaction Get(int id);
    void Add(Transaction Transaction);
    void Update(Transaction Transaction);
    void Delete(int id);
}
