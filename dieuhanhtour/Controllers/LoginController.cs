using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dieuhanhtour.Data.Utilities;
namespace dieuhanhtour.Controllers
{
    public class LoginController : Controller
    {
        private ILoginRepository _loginRepository;
        private IUserRepository _userRepository;
        MaHoaSHA1 sha1;

        // GET: Login
        public LoginController (ILoginRepository loginRepository, IUserRepository userRepository)
        {
            _loginRepository = loginRepository;
            _userRepository = userRepository;
            sha1 = new MaHoaSHA1();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _loginRepository.login(model.Username, model.Password, "001");
                if (result == null)
                {
                    ModelState.AddModelError("", "Tài khoản này không tồn tại");
                }
                else
                {
                    if (!result.Trangthai)
                    {
                        ModelState.AddModelError("", "Tài khoản này đã bị khóa");
                    }
                    string modelPass = sha1.EncodeSHA1(model.Password);
                    if (result.Password != modelPass)
                    {
                        ModelState.AddModelError("", "Mật khẩu không đúng");
                    }
                    if (result.Password == modelPass)
                    {
                        var user = _userRepository.GetById(model.Username);
                        HttpContext.Session.SetString("username", model.Username);
                        HttpContext.Session.SetString("password", model.Password);
                        HttpContext.Session.SetString("hoten", result.Hoten);
                        HttpContext.Session.SetString("phong", result.Maphong);
                        HttpContext.Session.SetString("chinhanh", user.chinhanh);
                        HttpContext.Session.SetString("dienthoai", String.IsNullOrEmpty(result.Dienthoai)?"":result.Dienthoai);
                        HttpContext.Session.SetString("macode", result.Macode);
                        HttpContext.Session.SetString("roleId", string.IsNullOrEmpty(result.RoleId)?"":result.RoleId);
                        HttpContext.Session.SetString("Newtour", user.Newtour.ToString());
                        HttpContext.Session.SetString("Dongtour", user.Dongtour.ToString());
                        HttpContext.Session.SetString("Danhmuc", user.Catalogue.ToString());
                        HttpContext.Session.SetString("Booking", user.Booking.ToString());
                        HttpContext.Session.SetString("Report", user.Report.ToString());
                        HttpContext.Session.SetString("Showprice", user.Showprice.ToString());
                        HttpContext.Session.SetString("Print", user.Print.ToString());
                        HttpContext.Session.SetString("Doixe", user.Doixe.ToString());
                        HttpContext.Session.SetString("Maybay", user.Maybay.ToString());
                        HttpContext.Session.SetString("Huongdan", user.Huongdan.ToString());
                        HttpContext.Session.SetString("Sales", user.Sales.ToString());
                        HttpContext.Session.SetString("Vetq", user.Vetq.ToString());
                        HttpContext.Session.SetString("Admin", user.Admin.ToString());
                        HttpContext.Session.SetString("khachle", user.khachle.ToString());
                        HttpContext.Session.SetString("khachdoan", user.khachdoan.ToString());
                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            HttpContext.Session.SetString("Email", user.Email.ToString());
                        }
                        DateTime ngaydoimk = Convert.ToDateTime(result.Ngaydoimk);
                        int kq = (DateTime.Now.Month - ngaydoimk.Month) + 12 * (DateTime.Now.Year - ngaydoimk.Year);
                        if (kq >= 3)
                        {
                            return View("changepass");
                        }
                        else if (result.Doimk)
                        {
                            return View("changepass");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Tour");
                        }

                    }
                }
            }
            return View();
        }
        public IActionResult changepass()
        {
            var user = _userRepository.GetById(HttpContext.Session.GetString("username"));
            changepassModel changpassmodel = new changepassModel
            {
                Username = user.Username
            };
            return View(changpassmodel);
        }
        [HttpPost]
        public IActionResult changepass(changepassModel model)
        {
            if (ModelState.IsValid)
            {
                string oldpass = HttpContext.Session.GetString("password");
                if (sha1.EncodeSHA1(oldpass) != sha1.EncodeSHA1(model.Password))
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                }
                else if(model.Newpassword!=model.Confirmpassword)
                {
                    ModelState.AddModelError("", "Mật khẩu nhập lại không đúng.");
                }
                else
                {
                    int result = _loginRepository.changepass(model.Username,sha1.EncodeSHA1(model.Newpassword));
                    if (result>0)
                    {
                        return RedirectToAction("Index", "home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể đổi mật khẩu.");
                    }
                }
              
            }
            return View();
        }

        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}