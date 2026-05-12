using Online_Shoppng.Models.Entity;

namespace Online_Shoppng.Models.Services
{
    public interface IfCategory
    {   
        int AddCategory(Category cat);
        int UpdateCategory(Category cat);   
        int DeleteCategory(int CategoryId);
        Category SearchCategory(int CategoryId);
        List<Category> GetAllCategories();
    }
}
