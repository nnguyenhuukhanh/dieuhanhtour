using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dieuhanhtour.Controllers
{
    public class PhongbanController : BaseController
    {
        private readonly IPhongbanRepository _phongbanRepository;
        public PhongbanController(IPhongbanRepository phongbanRepository)
        {
            _phongbanRepository = phongbanRepository;
        }
        public IActionResult Index(string searchString, int? page = 1)
        {
            HttpContext.Session.SetString("urlPhongban", UriHelper.GetDisplayUrl(Request));
            searchString = String.IsNullOrEmpty(searchString) ? "" : searchString;
            if (!_phongbanRepository.Any())
                return View("Empty");
            var pb = _phongbanRepository.ListPhongban(searchString, page);
            ViewBag.phongban = pb;
            ViewData["CurrentFilter"] = searchString;
            return View(pb);
        }
        public IActionResult Create()
        {
            Trangthai(true);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("maphong,tenphong,macode,trangthai")] Phongban phongban)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    phongban.maphong = phongban.maphong.ToUpper();
                    phongban.tenphong = phongban.tenphong.ToUpper();
                    phongban.macode = phongban.macode ?? "";
                    var result = _phongbanRepository.Create(phongban);
                    if (result == null)
                    {
                        SetAlert("Thêm khối / phòng không thành công", "error");
                    }
                    else
                    {
                        SetAlert("Thêm khối / phòng thành công", "success");
                    }
                    return Redirect(HttpContext.Session.GetString("urlPhongban"));
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            return View(phongban);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Phongban phongban = await _phongbanRepository.GetByIdAsync(id);
            if (phongban == null)
            {
                return NotFound();
            }
            PhongbanViewModel phongbanViewModel = new PhongbanViewModel
            {
                maphong = phongban.maphong,
                tenphong = phongban.tenphong,
                trangthai = phongban.trangthai,
                macode = phongban.macode
            };
            Trangthai(phongbanViewModel.trangthai);
            return View(phongbanViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("maphong,tenphong,trangthai,macode")] PhongbanViewModel phongban)
        {
            if (id != phongban.maphong)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Phongban phongbanChange = _phongbanRepository.GetById(phongban.maphong);
                phongbanChange.maphong = phongban.maphong;
                phongbanChange.tenphong = phongban.tenphong.ToUpper();
                phongbanChange.trangthai = phongban.trangthai;
                phongbanChange.macode = phongban.macode??"";
                var result = _phongbanRepository.Update(phongbanChange);
                if (result == null)
                {
                    SetAlert("Cập nhật khối / phòng không thành công", "error");
                }
                else
                {
                    SetAlert("Cập nhật khối / phòng thành công", "success");
                }
                return Redirect(HttpContext.Session.GetString("urlPhongban"));
            }
            return View(phongban);
        }
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var phong = _phongbanRepository.GetById(id);
            if (phong == null)
            {
                return NotFound();
            }
            var result = _phongbanRepository.Delete(phong);
            if (result == null)
            {
                SetAlert("Xóa khối / phòng không thành công.", "error");
            }
            else
            {
                SetAlert("Xóa khối / phòng thành công.", "success");
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PhongbanExists(string maphong)
        {
            bool result = false;
            var pb = _phongbanRepository.GetById(maphong);
            if (pb == null)
                result = true;
            return Json(result);
        }

        public void Trangthai(bool select)
        {
            List<SelectListItem> trangthai = new List<SelectListItem>();
            trangthai.Add(new SelectListItem { Text = "Kích hoạt", Value = "True" });
            trangthai.Add(new SelectListItem { Text = "Khóa", Value = "False" });

            ViewBag.trangthai = new SelectList(trangthai, "Value", "Text", select);
        }
    }
}