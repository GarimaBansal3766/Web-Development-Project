using Microsoft.EntityFrameworkCore;
using Online_Shoppng.Models.Entity.Online_Shoppng.Models.Entity;

namespace Online_Shoppng.Models.Entity
{
    public class OnlineShoppingDBcontext:DbContext
    {
        public OnlineShoppingDBcontext(DbContextOptions<OnlineShoppingDBcontext> options) : base(options) 
        { 
        
        }
        public DbSet<Category> CategoriesDetails { get; set; }
        public DbSet<Product> ProductsDetails {  get; set; }
        public DbSet<login> loginDetails {  get; set; }
      
        public DbSet<Order> OrderDetails { get; set; }
        public DbSet<OrdersMaster> OrdersMaster { get; set; }
        public DbSet<Cart> Carts { get; set; }

    }
}
