using System.ComponentModel.DataAnnotations;

namespace Online_Shoppng.Models.Entity
{
    public class OrdersMaster
    {
        [Key]
        public int OrderMasterId { get; set; }

        public string Username { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal GrandTotal { get; set; }

        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
    }

}
