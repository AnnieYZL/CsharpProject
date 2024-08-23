using SV21T1080067.DomainModels;

namespace SV21T1080067.Web.Models
{
    public class ProductSearchResult : PaginationSearchResult
    {
            public int CategoryId { get; set; } = 0;
            public int SupplierId { get; set; } = 0;
            public decimal MinPrice { get; set; } = 0;
            public decimal MaxPrice { get; set; } = 0;
            public required List<Product>? Data { get; set; }
        
    }
}
