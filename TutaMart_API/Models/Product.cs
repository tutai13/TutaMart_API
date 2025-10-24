using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TutaMart_API.Models
{
    public class Product
    {
        [Key]
        public int SanPhamID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        public string TenSanPham { get; set; }

        [StringLength(500)]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal Gia { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn phải >= 0")]
        public int SoLuongTon { get; set; }

        [StringLength(255)]
        public string? HinhAnh { get; set; }
        [ForeignKey("LoaiID")]

        public int LoaiID { get; set; }

        [JsonIgnore]
        public  Category? Loai { get; set; }
    }
}
