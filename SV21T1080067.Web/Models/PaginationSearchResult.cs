using SV21T1080067.DomainModels;

namespace SV21T1080067.Web.Models
{

    /// <summary>
    /// Lớp cơ sở cho các kết quả tìm kiếm, hiển thị dưới dạng phân trang
    /// </summary>
    public class PaginationSearchResult
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; } = 0;

        public int PageCount
        {
            get
            {
                if(PageSize ==0) return 1;
                int n = RowCount / PageSize;
                if(RowCount % PageSize > 0) n+=1;
                return n;
            }
        }
    }
    public class CustomerSearchResult : PaginationSearchResult
    {
        public List<Customer>? Data { get; set; }
    }
    public class CategorySearchResult : PaginationSearchResult
    {
        public List<Category>? Data { get; set; }
    }
    public class ShipperSearchResult : PaginationSearchResult
    {
        public List<Shipper>? Data { get; set; }
    }
    public class SupplierSearchResult : PaginationSearchResult
    {
        public List<Supplier>? Data { get; set; }
    }
    
}
