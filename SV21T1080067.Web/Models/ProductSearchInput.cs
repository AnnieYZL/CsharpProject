namespace SV21T1080067.Web.Models
{
    /// <summary>
    /// Đầu vào tìm kiếm dữ liệu để nhận dữ liệu dưới dạng phân trang
    /// </summary>
    public class ProductSearchInput
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 0;
        public string SearchValue { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public int SupplierId { get; set; } = 0;
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = 0;

    }
}