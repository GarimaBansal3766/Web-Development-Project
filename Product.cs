using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shoppng.Models.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Online_Shoppng.Models.Entity
    {
        public class Product
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
            public int ProductId { get; set; }
            public int CategoryId { get; set; }

            [ForeignKey("CategoryId")]
            public Category Category { get; set; }
            public string ProductName { get; set; }
            public float ProductPrice { get; set; }
            public string ProductDescription { get; set; }
            public int ProductDiscount { get; set; }
            public string BrandName { get; set; }
            public string Productimage1 { get; set; }
            public string Productimage2 { get; set; }
            public string Productimage3 { get; set; }
            
        }
    }

}
