using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TutaMart_API.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        // Product Name
        [Required]
        [StringLength(150)]
        public string ProductName { get; set; }

        // SKU
        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        // Cost Price
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        // Selling Price
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }

        // Initial Quantity
        [Required]
        public int Quantity { get; set; }

        // Supplier
        [StringLength(200)]
        public string? Supplier { get; set; }

        // Image path / filename
        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }

        // Khóa ngoại Category
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        
        public virtual Category? Category { get; set; }
    }
}
