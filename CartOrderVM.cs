using System.Collections.Generic;

namespace Online_Shoppng.Models.ViewModels
{
    public class CartOrderVM
    {
        // 🔹 List of products coming from cart
        public List<CartItemVM> Items { get; set; }

        // 🔹 User-entered details (from Order.cshtml)
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
    }
}
