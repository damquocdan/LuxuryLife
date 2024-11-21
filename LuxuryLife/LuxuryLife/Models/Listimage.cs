namespace LuxuryLife.Models
{
    public class Listimage
    {
        public int Id { get; set; } // Khóa chính
        public string? Name { get; set; } // Tên danh sách hình ảnh (tuỳ chọn)
        public ICollection<Tour>? Images { get; set; } // Quan hệ 1-n với bảng Image
    }
}
