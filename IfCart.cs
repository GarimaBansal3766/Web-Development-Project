using Online_Shoppng.Models.Entity;

public interface IfCart
{
    void AddToCart(Cart cart);
    List<Cart> GetCartByUsername(string username);
    void RemoveFromCart(int cartId);
    void ClearCart(string username);
    void UpdateQuantity(int cartId, int quantity);

}
