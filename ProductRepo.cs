using Microsoft.EntityFrameworkCore;
using Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Entity.Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Services;

namespace Online_Shoppng.Models.Repository
{
    public class ProductRepo : IfProducts
    {
        private OnlineShoppingDBcontext _context;

        public ProductRepo(OnlineShoppingDBcontext context)
        {
            _context = context;
        }
        public int AddProduct(Product pro)
        {
            _context.Add(pro);
            int i = _context.SaveChanges();
            return i;
        }
        public bool DeleteProduct(int id)
        {
            var product = _context.ProductsDetails.Find(id);
            if (product == null) return false;

            _context.ProductsDetails.Remove(product);
            _context.SaveChanges();
            return true;
        }

        public List<Product> GetProducts()
        {
            return _context.ProductsDetails.ToList();
        }

        public Product SearchProduct(int ProductId)
        {
            Product ProObj = _context.ProductsDetails.Find(ProductId);
            return ProObj;      
        }

        public int UpdateProduct(Product pro)
        {
            Product proObj = _context.ProductsDetails.Find(pro.ProductId);
            proObj.ProductName=pro.ProductName;
            proObj.ProductDescription=pro.ProductDescription;
            proObj.ProductPrice=pro.ProductPrice;
            proObj.ProductDiscount=pro.ProductDiscount;
            proObj.BrandName=pro.BrandName;
            if (!string.IsNullOrEmpty(pro.Productimage1))
            {
                proObj.Productimage1 = pro.Productimage1;
            }
            if (!string.IsNullOrEmpty(pro.Productimage2))
            {
                proObj.Productimage2 = pro.Productimage2;
            }
            if (!string.IsNullOrEmpty(pro.Productimage3))
            {
                proObj.Productimage3 = pro.Productimage3;
            }
            int i = _context.SaveChanges();
            return i;
        }
       



    }
}
