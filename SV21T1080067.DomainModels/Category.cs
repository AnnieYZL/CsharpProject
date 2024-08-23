using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1080067.DomainModels
{
    /// <summary>
    /// Lớp biểu diễn thông tin về loại hàng
    /// </summary>
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
