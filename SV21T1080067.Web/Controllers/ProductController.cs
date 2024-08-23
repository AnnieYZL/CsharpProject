using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1080067.BusinessLayers;
using SV21T1080067.DomainModels;
using SV21T1080067.Web.Models;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1080067.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SEARCH_CONDITION = "product_search"; //Tên biến dùng để lưu trong session


        public IActionResult Index()
        {
            ProductSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(SEARCH_CONDITION);
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryId = 0,
                    SupplierId = 0,
                    MinPrice = 0,
                    MaxPrice = 0
                };
            }
            return View(input);
        }

        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryId, input.SupplierId, input.MinPrice, input.MaxPrice);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                CategoryId = input.CategoryId,
                SupplierId = input.SupplierId,
                MinPrice = input.MinPrice,
                MaxPrice = input.MaxPrice,
                Data = data
            };
            ApplicationContext.SetSessionData(SEARCH_CONDITION, input);
            return View(model);
        }


        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Mặt hàng";
            Product product = new Product()
            {
                ProductID = 0
            };
            return View("Edit", product);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Mặt hàng";
            ViewBag.IsEdit = true;
            Product? model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");

            if (string.IsNullOrEmpty(model.Photo))
                model.Photo = "nophoto.png";
            return View(model);
        }
        public IActionResult Delete(int id = 0)
        {
            //Nếu lời gọi là post thì thực hiện xóa
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            //Nếu lời gọi là get thì hiển thị mặt hàng cần xóa
            var product = ProductDataService.GetProduct(id);
            if (product == null) return RedirectToAction("Index");

            ViewBag.AllowDelete = !ProductDataService.InUsedProduct(id);

            return View(product);
        }

        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên không được để trống");

            if (data.CategoryID == 0)
                ModelState.AddModelError(nameof(data.CategoryID), "Danh mục hàng không được để trống");

            if (data.SupplierID == 0)
                ModelState.AddModelError(nameof(data.SupplierID), "Nhà cung cấp không được để trống");

            if (string.IsNullOrWhiteSpace(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống");

            if (data.Price == 0)
                ModelState.AddModelError(nameof(data.Price), "Giá tiền không được để trống");

            //Xử lý với ảnh upload (nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho employee)
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.WebRootPath, @"images\products"); //đường dẫn đến thư mục lưu file
                string filePath = Path.Combine(folder, fileName); //Đường dẫn đến file cần lưu D:\images\products\photo.png

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            if (!ModelState.IsValid)
            {
                    return View("Edit", data);
            }
            if (data.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(data);
            }
            else
            {
                bool result = ProductDataService.UpdateProduct(data);

            }
            return RedirectToAction("Index");
        }

        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    ProductPhoto model = new ProductPhoto()
                    {
                        ProductID = id,
                        Photo = "nophoto.png",
                        PhotoID = 0
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh của mặt hàng";
                    var data = ProductDataService.GetPhoto(photoId);
                    if (data == null)
                        return RedirectToAction("Edit", new { id = id });

                    return View(data);
                case "delete":
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto model, IFormFile? uploadPhoto)
        {

            if (string.IsNullOrWhiteSpace(model.DisplayOrder.ToString()))
                ModelState.AddModelError(nameof(model.DisplayOrder), "Thứ tự hiển thị không được để trống");

            List<ProductPhoto> productPhotos = ProductDataService.ListPhotos(model.ProductID);

            bool displayOrder = false;
            foreach (ProductPhoto item in productPhotos)
            {
                if (item.DisplayOrder == model.DisplayOrder && model.PhotoID != item.PhotoID)
                {
                    displayOrder = true;
                    break;
                }

            }
            if (displayOrder)
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị đã được sử dụng. Vui lòng chọn thứ tự khác !");

            //Xử lý với ảnh upload (nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho employee)
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.WebRootPath, @"images\products\photos"); //đường dẫn đến thư mục lưu file
                string filePath = Path.Combine(folder, fileName); //Đường dẫn đến file cần lưu D:\images\products\photo.png

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                model.Photo = fileName;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.PhotoID == 0 ? "Bổ sung ảnh cho mặt hàng" : "Cập nhật ảnh của mặt hàng";
                return View("Photo", model);
            }



            if (model.PhotoID == 0)
            {
                ProductDataService.AddPhoto(model);
            }
            else
            {
                ProductDataService.UpdatePhoto(model);
            }

            return RedirectToAction("Edit", new { id = model.ProductID });
        }
        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    ProductAttribute model = new ProductAttribute()
                    {
                        ProductID = id,
                        AttributeID = 0
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Cập nhật thuộc tính của mặt hàng";
                    var data = ProductDataService.GetAttribute(attributeId);
                    if (data == null)
                        return RedirectToAction("Edit", new { id = id });
                    return View(data);
                case "delete":
                    //TODO: Xóa thuộc tính có mã photoID (Xóa trực tiếp, không cần phải xác nhận)

                    bool result = ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        
        }


        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute model)
        {

            if (string.IsNullOrWhiteSpace(model.AttributeName))
                ModelState.AddModelError(nameof(model.AttributeName), "Tên thuộc tính không được để trống");

            if (string.IsNullOrWhiteSpace(model.AttributeValue))
                ModelState.AddModelError(nameof(model.AttributeValue), "Giá trị thuộc tính không được để trống");

            List<ProductAttribute> productAttributes = ProductDataService.ListAttributes(model.ProductID);

            bool displayOrder = false;
            foreach (ProductAttribute item in productAttributes)
            {
                if (item.DisplayOrder == model.DisplayOrder && model.AttributeID != item.AttributeID)
                {
                    displayOrder = true;
                    break;
                }

            }
            if (displayOrder)
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị đã được sử dụng. Vui lòng chọn thứ tự khác !");


            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.AttributeID == 0 ? "Bổ sung thuộc tính" : "Thay đổi thuộc tính";
                return View("Attribute", model);
            }
            if (model.AttributeID == 0)
            {
                ProductDataService.AddAttribute(model);

            }
            else
            {
                ProductDataService.UpdateAttribute(model);
            }

            return RedirectToAction("Edit", new { id = model.ProductID });
        }
    }
}
