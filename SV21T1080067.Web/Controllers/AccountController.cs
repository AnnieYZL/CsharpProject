using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1080067.BusinessLayers;
using SV21T1080067.DomainModels;
using SV21T1080067.Web;
using SV21T1080067.Web.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1080067.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username = "", string password = "")
        {
            ViewBag.UserName = username;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập tên và mật khẩu!");
                return View();
            }

            var userAccount = UserAccountService.Authorize(username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại!");
                return View();
            }

            //Đăng nhập thành công, tạo dữ liệu để lưu thông tin đăng nhập
            var userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData = "",
                Roles = userAccount.RoleNames.Split(',').ToList()
            };
            //Thiết lập phiên đăng nhập cho tài khoản
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            //Redirec về trang chủ sau khi đăng nhập
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string userName = "", string oldPassword = "", string newPassword = "", string confirmPassword = "")
        {
            ViewBag.oldPassword = oldPassword;

            if (string.IsNullOrWhiteSpace(oldPassword))
                ModelState.AddModelError("oldPassword", "Vui lòng nhập mật khẩu hiện tại!");

            if (string.IsNullOrWhiteSpace(newPassword))
                ModelState.AddModelError("newPassword", "Vui lòng nhập mật khẩu mới!");

            if (string.IsNullOrWhiteSpace(confirmPassword))
                ModelState.AddModelError("confirmPassword", "Bạn phải xác nhận mật khẩu mới!");

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Mật khẩu xác nhận không chính xác!");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isChangePassword = UserAccountService.ChangePassword(userName, oldPassword, newPassword);
            if (!isChangePassword)
            {
                ModelState.AddModelError("oldPassword", "Mật khẩu hiện tại không chính xác");
                return View();
            }
            
            else
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công. Bạn cần đăng nhập lại!";
                return View();
            }

        }
    }
}
