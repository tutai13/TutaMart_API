using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TutaMart_API.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(500)]
        public string Detail { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [StringLength(200)]
        public string Images { get; set; }

        // Khóa ngoại liên kết với Loai
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        
        public virtual Category? Category { get; set; }
    }
}
