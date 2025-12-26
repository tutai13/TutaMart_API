using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TutaMart_API.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [JsonIgnore]

        public ICollection<Product>? Products { get; set; }
    }
}
