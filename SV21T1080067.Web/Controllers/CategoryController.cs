using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1080067.BusinessLayers;
using SV21T1080067.DomainModels;
using SV21T1080067.Web.Models;

namespace SV21T1080067.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SEARCH_CONDITION = "category_search"; //Tên biến dùng để lưu trong session

        public IActionResult Index(int page = 1, string searchValue = "")
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
            var data = CommonDataService.ListOfCategories(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new CategorySearchResult()
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
            ViewBag.Title = "Bổ sung Loại hàng";
            Category category = new Category()
            {
                CategoryID = 0
            };
            return View("Edit", category);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Loại hàng";
            Category? category = CommonDataService.GetCategory(id);
            if (category == null)
                return RedirectToAction("Index");
            return View(category);
        }
        [HttpPost]

        public IActionResult Save(Category data)
        {
            ViewBag.Title = data.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhật thông tin loại hàng";
            //Kiểm tra dữ liệu đầu vào có hợp lệ hay không => Ghi Task List
            if (string.IsNullOrWhiteSpace(data.CategoryName))
                ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng không được để trống");
            data.Description = data.Description ?? "";
            //Nếu tồn tại lỗi => Trả về view để người sd nhập lại cho đúng
            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            //Gọi chức năng xử lý dưới tầng tác nghiệp nếu quá trình kiểm soát lỗi không phát hiện lỗi đầu vào
            if (data.CategoryID == 0)
            {
                CommonDataService.AddCategory(data);
            }
            else
            {
                CommonDataService.UpdateCategory(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            //Nếu lời gọi là post thì thực hiện xóa
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            //Nếu lời gọi là get thì hiển thị loại hàng cần xóa
            var category = CommonDataService.GetCategory(id);
            if (category == null) return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedCategory(id);

            return View(category);
        }
    }
}
