namespace Stockify.Business;
using Stockify.Models;

public interface ITransactionService {
    void Add(Transaction Transaction);
    void Delete(int id);
    void Update(Transaction Transaction);
    Transaction Get (int id);
    List<Transaction> GetAll();
}