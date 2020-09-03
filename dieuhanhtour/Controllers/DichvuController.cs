using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.ViewModel;

namespace dieuhanhtour.Controllers
{
    public class DichvuController : BaseController
    {
        //private readonly qltourContext _context;
        private readonly IDichvuRepository _dichvuRepository;

        public DichvuController( IDichvuRepository dichvuRepository)
        {
            _dichvuRepository = dichvuRepository;
        }

        // GET: Dichvu
        //public async Task<IActionResult> Index(string searchString)
        //{
        //    searchString = !String.IsNullOrEmpty(searchString) ? searchString : "";
        //    var model = await _dichvuRepository.FindAsync(x => x.Iddichvu.Contains(searchString));
        //    @ViewData["currentFilter"] = searchString;
        //    return View(model);
        //}
        public async Task<IActionResult> Index()
        {
            var model = await _dichvuRepository.GetAllAsync();
            return View(model);
        }

        // GET: Dichvu/Create
        public IActionResult Create()
        {
             Trangthai(true);
            return View();
        }

        // POST: Dichvu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Iddichvu,Tendv,Trangthai")] Dichvu dichvu)
        {
            if (ModelState.IsValid)
            {
                var result = _dichvuRepository.Create(dichvu);
                if (result == null)
                {
                    SetAlert("Thêm dịch vụ không thành công", "error");
                }
                else
                {
                    SetAlert("Thêm dịch vụ thành công", "success");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dichvu);
        }

        // GET: Dichvu/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Dichvu dichvu = await _dichvuRepository.GetByIdAsync(id);
            if (dichvu == null)
            {
                return NotFound();
            }
            DichvuViewModel dichvuViewModel = new DichvuViewModel
            {
                Iddichvu = dichvu.Iddichvu,
                Tendv = dichvu.Tendv,
                Trangthai = dichvu.Trangthai
            };
            Trangthai(dichvuViewModel.Trangthai);
            return View(dichvuViewModel);
        }

        // POST: Dichvu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Iddichvu,Tendv,Trangthai")] DichvuViewModel dichvu)
        {
            if (id != dichvu.Iddichvu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Dichvu d = await _dichvuRepository.GetByIdAsync(dichvu.Iddichvu);
                    d.Iddichvu = dichvu.Iddichvu;
                    d.Tendv = dichvu.Tendv;
                    d.Trangthai = dichvu.Trangthai;
                    var result = _dichvuRepository.Update(d);
                    if (result == null)
                    {
                        SetAlert("Cập nhật dịch vụ không thành công", "error");
                    }
                    else
                    {
                        SetAlert("Cập nhật dịch vụ thành công", "success");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }              
                return RedirectToAction(nameof(Index));
            }
            return View(dichvu);
        }

        // GET: Dichvu/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichvu = await _dichvuRepository.GetByIdAsync(id);
            if (dichvu == null)
            {
                return NotFound();
            }
            var result = _dichvuRepository.Delete(dichvu);
            if (result == null)
            {
                SetAlert("Xóa dịch vụ không thành công", "error");
            }
            else
            {
                SetAlert("Xóa dịch vụ thành công", "success");
            }
            return RedirectToAction(nameof(Index));
        }     

        public async Task<IActionResult> DichvuExists(string Iddichvu)
        {
            bool result = false;
            var dv = await _dichvuRepository.GetByIdAsync(Iddichvu);
            if (dv == null)
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
