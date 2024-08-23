using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1080067.BusinessLayers;
using SV21T1080067.DomainModels;
using SV21T1080067.Web.Models;

namespace SV21T1080067.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SEARCH_CONDITION = "shipper_search"; //Tên biến dùng để lưu trong session


        public IActionResult Index()
        {
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SEARCH_CONDITION);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(SEARCH_CONDITION, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Người giao hàng";
            Shipper shipper = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", shipper);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Người giao hàng";
            Shipper? shipper = CommonDataService.GetShipper(id);
            if (shipper == null)
                return RedirectToAction("Index");
            return View(shipper);
        }
        [HttpPost]
        public IActionResult Save(Shipper data)
        {
            ViewBag.Title = data.ShipperID == 0 ? "Bổ sung Người giao hàng" : "Cập nhật thông tin Người giao hàng hàng";
            //Kiểm tra dữ liệu đầu vào có hợp lệ hay không => Ghi Task List
            if (string.IsNullOrWhiteSpace(data.ShipperName))
                ModelState.AddModelError(nameof(data.ShipperName), "Tên người giao hàng không được để trống");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được để trống");
            //Nếu tồn tại lỗi => Trả về view để người sd nhập lại cho đúng
            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            //Gọi chức năng xử lý dưới tầng tác nghiệp nếu quá trình kiểm soát lỗi không phát hiện lỗi đầu vào
            if (data.ShipperID == 0)
            {
                CommonDataService.AddShipper(data);
            }
            else
            {
                CommonDataService.UpdateShipper(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            //Nếu lời gọi là post thì thực hiện xóa
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            //Nếu lời gọi là get thì hiển thị người giao hàng cần xóa
            var shipper = CommonDataService.GetShipper(id);
            if (shipper == null) return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedShipper(id);

            return View(shipper);
        }
    }
}
