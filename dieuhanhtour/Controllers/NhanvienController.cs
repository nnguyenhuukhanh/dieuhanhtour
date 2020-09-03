using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using dieuhanhtour.Data.Utilities;

namespace dieuhanhtour.Controllers
{
    public class NhanvienController : BaseController
    {
       // private readonly qltourContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IChinhanhRepository _chinhanhRepository;
        private readonly IPhongbanRepository _phongbanRepository;
        MaHoaSHA1 sha1;
        public NhanvienController( IUserRepository userRepository,IChinhanhRepository chinhanhRepository,IPhongbanRepository phongbanRepository)
        {
           // _context = context;
            _userRepository = userRepository;
            _chinhanhRepository = chinhanhRepository;
            _phongbanRepository = phongbanRepository;
            sha1 = new MaHoaSHA1();
        }

        // GET: Nhanvien
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Users.ToListAsync());
            var users = await _userRepository.GetAllAsync();
            HttpContext.Session.SetString("urlNhanvien", UriHelper.GetDisplayUrl(Request));
            return View(users);
        }

        public ActionResult Create()
        {
            var user = new Users();
            user.Active = true;
            listChinhanh(HttpContext.Session.GetString("chinhanh"));
            listPhong("");
            return View(user);
        }
       [HttpPost]
        public ActionResult Create(Users entity)
        {
            if (ModelState.IsValid)
            {
                entity.Password = sha1.EncodeSHA1(entity.Password);
                entity.Hoten = entity.Hoten.ToUpper();
                var result = _userRepository.Create(entity);
                int n =  _userRepository.createUsers_qltaikhoan(entity.Username, entity.Hoten.ToUpper(),entity.Password,entity.Maphong, entity.chinhanh);
                if (result != null)
                {
                    SetAlert("Tạo user thành công", "success");
                }
                else
                {
                    SetAlert("Tạo user không thành công", "error");
                }
            }
            return Redirect(HttpContext.Session.GetString("urlNhanvien"));
        }
       
        // GET: Nhanvien/Edit/5
       // public async Task<IActionResult> Edit(string id)
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users =  _userRepository.GetById(id);
            users.Password = "";
            listChinhanh(users.chinhanh);
            listPhong(users.Maphong);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Nhanvien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(Users entity)
        {            
            if (ModelState.IsValid)
            {
                Users users = _userRepository.GetById(entity.Username);
                string pass = "";
                //if(!string.IsNullOrWhiteSpace(entity.Password))
                //{
                //    pass = sha1.EncodeSHA1(entity.Password);
                //}
                users.Username = entity.Username;
                users.Hoten = entity.Hoten.ToUpper();
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    users.Password = sha1.EncodeSHA1(entity.Password);
                    pass = users.Password;
                }
                users.Maphong = entity.Maphong;
                users.chinhanh = entity.chinhanh;                
                users.Newtour = entity.Newtour;
                users.Dongtour = entity.Dongtour;
                users.Catalogue = entity.Catalogue;
                users.Booking = entity.Booking;
                users.Report = entity.Report;
                users.Showprice = entity.Showprice;
                users.Print = entity.Print;
                users.Doixe = entity.Doixe;
                users.Huongdan = entity.Huongdan;
                users.Maybay = entity.Maybay;
                users.Vetq = entity.Vetq;
                users.Sales = entity.Sales;
                users.Active = entity.Active;
                users.Admin = entity.Admin;
                users.khachle = entity.khachle;
                users.khachdoan = entity.khachdoan;
                var result = _userRepository.Update(users);
                int n = _userRepository.editUsers_qltaikhoan(users.Username,users.Hoten, pass, users.Maphong, users.chinhanh);
                if (result == null)
                {
                    SetAlert("Cập nhật quyền không thành công.", "error");
                }
                else
                {
                    SetAlert("Cập nhật quyền thành công.", "success");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

      
        public void listChinhanh(string selected)
        {
            List<Dmchinhanh> chinhanh = _chinhanhRepository.ListChinhanh().ToList();
            ViewBag.chinhanh = new SelectList(chinhanh, "Macn", "Macn", selected);
        }
        public void listPhong(string select)
        {
            List<Phongban> phongban = _phongbanRepository.ListPhongban().ToList();
            ViewBag.phongban = new SelectList(phongban, "maphong", "tenphong", select);
        }

        #region Kiểm tra sự tồn tại của nhân viên
        public async Task<IActionResult> UserExists(string Username)
        {
            bool result = false;
            var u = await _userRepository.GetByIdAsync(Username);
            if (u == null)
                result = true;

            return Json(result);
        }
        #endregion
    }
}
