using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TutaMart_API.Models
{
    public class Category
    {
        [Key]
        public int LoaiID { get; set; }

        [Required(ErrorMessage = "Tên loại không được để trống")]
        [StringLength(100)]
        public string TenLoai { get; set; }

        [StringLength(255)]
        public string? MoTa { get; set; }
        [JsonIgnore]
        public ICollection<Product>? SanPhams { get; set; }
    }
}
