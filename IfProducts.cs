using Online_Shoppng.Models.Entity.Online_Shoppng.Models.Entity;

namespace Online_Shoppng.Models.Services
{
    public interface IfProducts
    {
        int AddProduct(Product pro);
        int UpdateProduct(Product pro);
        bool DeleteProduct(int ProductId);
        Product SearchProduct(int ProductId);
        List<Product> GetProducts();

    }
}
