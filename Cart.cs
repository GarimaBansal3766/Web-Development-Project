using Online_Shoppng.Models.Entity.Online_Shoppng.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shoppng.Models.Entity
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public string Username { get; set; }

        [ForeignKey("Username")]
        public login Login { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
