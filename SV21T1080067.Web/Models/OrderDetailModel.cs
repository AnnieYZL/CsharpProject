using SV21T1080067.DomainModels;

namespace SV21T1080067.Web.Models
{
    public class OrderDetailModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
