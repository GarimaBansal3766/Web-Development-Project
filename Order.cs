using Online_Shoppng.Models.Entity.Online_Shoppng.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shoppng.Models.Entity
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int? OrderMasterId { get; set; }

        public string Username { get; set; }

        [ForeignKey("Username")]
        public login Login { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string TrackingNumber { get; set; }

        // ✅ NEW (correctly added)
    }
}
