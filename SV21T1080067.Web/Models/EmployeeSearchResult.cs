using SV21T1080067.DomainModels;
using SV21T1080067.Web.Models;

namespace SV21T1080067.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm nhân viên
    /// </summary>
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }

}