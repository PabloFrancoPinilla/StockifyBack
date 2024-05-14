using Stockify.Models;

namespace Stockify.Data;

public interface ITransactionRepository{
    List<Transaction> GetAll();
    Transaction Get(int id);
    void Add (Transaction Transaction);
    void Update (Transaction Transaction);
    void Delete (int id);
}
