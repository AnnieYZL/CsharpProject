using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1080067.BusinessLayers;
using SV21T1080067.DomainModels;
using SV21T1080067.Web.Models;

namespace SV21T1080067.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SEARCH_CONDITION = "supplier_search"; //Tên biến dùng để lưu trong session


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
            var data = CommonDataService.ListOfSuppliers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new SupplierSearchResult()
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
            ViewBag.Title = "Bổ sung Nhà cung cấp";
            Supplier supplier = new Supplier()
            {
                SupplierID = 0
            };
            return View("Edit", supplier);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Nhà cung cấp";
            Supplier? supplier = CommonDataService.GetSupplier(id);
            if (supplier == null)
                return RedirectToAction("Index");
            return View(supplier);
        }
        public IActionResult Save(Supplier data)
        {
            ViewBag.Title = data.SupplierID == 0 ? "Bổ sung Nhà cung cấp" : "Cập nhật thông tin Nhà cung cấp";
            //Kiểm tra dữ liệu đầu vào có hợp lệ hay không => Ghi Task List
            if (string.IsNullOrWhiteSpace(data.SupplierName))
                ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn tỉnh thành!");

            data.Phone = data.Phone ?? "";
            data.Email = data.Email ?? "";
            data.Address = data.Address ?? "";
            //Nếu tồn tại lỗi => Trả về view để người sd nhập lại cho đúng
            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            //Gọi chức năng xử lý dưới tầng tác nghiệp nếu quá trình kiểm soát lỗi không phát hiện lỗi đầu vào
            if (data.SupplierID == 0)
            {
                CommonDataService.AddSupplier(data);
            }
            else
            {
                CommonDataService.UpdateSupplier(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            //Nếu lời gọi là post thì thực hiện xóa
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }
            //Nếu lời gọi là get thì hiển thị khách hàng cần xóa
            var supplier = CommonDataService.GetSupplier(id);
            if (supplier == null) return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedSupplier(id);

            return View(supplier);
        }
    }
}
