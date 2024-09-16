
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Product
    {
        [Key]
        [Range(100000, 999999)]  // Ensures ProductId is a 6-digit number
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
