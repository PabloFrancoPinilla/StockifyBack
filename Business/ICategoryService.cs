namespace Stockify.Business;
using Stockify.Models;

public interface ICategoryService {
    void Add(Category Category);
    void Delete(int id);
    void Update(Category Category);
    Category Get (int id);
    List<Category> GetAll();
    List<CategoryDto> GetCategoriesByInventoryId(int id);
}