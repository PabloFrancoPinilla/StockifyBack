using Stockify.Models;

namespace Stockify.Data;

public interface ICategoryRepository{
    List<Category> GetAll();
    List<CategoryDto> GetCategoriesByInventoryId(int id);
    Category Get(int id);
    void Add (Category Category);
    void Update (Category Category);
    void Delete (int id);
}
