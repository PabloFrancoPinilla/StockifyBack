namespace Stockify.Business;
using Stockify.Data;
using Stockify.Models;
public class CategoryService : ICategoryService
{
private readonly ICategoryRepository _CategoryRepository;
public CategoryService(ICategoryRepository CategoryRepository){
    _CategoryRepository = CategoryRepository;
}
public Category Get(int id) => _CategoryRepository.Get(id);
public void Update(Category Category) => _CategoryRepository.Update(Category);
public void Delete(int id)=>_CategoryRepository.Delete(id);
public List<Category> GetAll () => _CategoryRepository.GetAll();
public List<CategoryDto> GetCategoriesByInventoryId(int id) => _CategoryRepository.GetCategoriesByInventoryId(id);
public void Add (Category Category) => _CategoryRepository.Add(Category);

}
