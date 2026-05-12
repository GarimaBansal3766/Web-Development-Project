using Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Services;

namespace Online_Shoppng.Models.Repository
{
    public class CategoryRepo : IfCategory
    {
        private OnlineShoppingDBcontext _context;

        public CategoryRepo(OnlineShoppingDBcontext context)
        {
            _context = context;
        }

        public int AddCategory(Category cat)
        {
            _context.Add(cat);
            int i = _context.SaveChanges();
            return i;
        }

        public int DeleteCategory(int CategoryId)
        {
            // Category CatObj = _context.CategoriesDetails.Find(CategoryId);
            Category CatObj = _context.CategoriesDetails.FirstOrDefault(s=> s.CategoryId == CategoryId);
            _context.CategoriesDetails.Remove(CatObj);
            int i = _context.SaveChanges();
            return i;
        }


        public List<Category> GetAllCategories()
        {
            return _context.CategoriesDetails.ToList();
        }

        public Category SearchCategory(int CategoryId)
        {
            Category CatObj = _context.CategoriesDetails.Find(CategoryId);
            return CatObj;
        }
       

        public int UpdateCategory(Category cat)
        {
            Category Catobj = _context.CategoriesDetails.Find(cat.CategoryId);
            Catobj.CategoryName = cat.CategoryName;
            if (!string.IsNullOrEmpty(cat.Categoryimage))
            {
                Catobj.Categoryimage = cat.Categoryimage;
            } 
            Catobj.IsActive = cat.IsActive;
            int i = _context.SaveChanges();
            return i;
        }
    }
}

