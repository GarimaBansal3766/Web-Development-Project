namespace Online_Shoppng.Models.ViewModels
{
    public class CartItemVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
