using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Interfaces;

namespace dieuhanhtour.Controllers
{
    public class TourkindController : BaseController
    {
        private readonly qltourContext _context;
        private readonly ITourkindRepository _tourkindRepository;

        public TourkindController(qltourContext context, ITourkindRepository tourkindRepository)
        {
            _context = context;
            _tourkindRepository = tourkindRepository;
        }

        // GET: Tourkind
        public async Task<IActionResult> Index(string searchString)
        {
            searchString = !String.IsNullOrEmpty(searchString) ? searchString : "";
            var model = await _tourkindRepository.FindAsync(x => x.TourkindInf.Contains(searchString));
            @ViewData["currentFilter"] = searchString;
            return View(model);
        }


        // GET: Tourkind/Create
        public IActionResult Create()
        {
            Tourkind tourkind = new Tourkind();
            int id = _tourkindRepository.GetAll().OrderByDescending(x => x.Id).Take(1).SingleOrDefault().Id;
            tourkind.Id = id + 1;
            return View(tourkind);
        }

        // POST: Tourkind/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,TourkindInf")] Tourkind tourkind)
        {
            if (ModelState.IsValid)
            {
                tourkind.TourkindInf = tourkind.TourkindInf.ToUpper();
                var result = _tourkindRepository.Create(tourkind);
                if (result == null)
                {
                    SetAlert("Thêm loại tour không thành công", "error");
                }
                else
                {
                    SetAlert("Thêm loại tour thànhcông", "success");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tourkind);
        }

        // GET: Tourkind/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourkind = await _context.Tourkind.FindAsync(id);
            if (tourkind == null)
            {
                return NotFound();
            }
            return View(tourkind);
        }

        // POST: Tourkind/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,TourkindInf")] Tourkind tourkind)
        {
            if (id != tourkind.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _tourkindRepository.Update(tourkind);
                if (result == null)
                {
                    SetAlert("Cập nhật loại tour không thành công", "error");
                }
                else
                {
                    SetAlert("Cập nhật loại tour thành công", "success");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(tourkind);
        }

        // GET: Tourkind/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
           
            var tourkind = await _tourkindRepository.GetByIdAsync(id);
              
            if (tourkind == null)
            {
                return NotFound();
            }
            var result = _tourkindRepository.Delete(tourkind);
            if (result == null)
            {
                SetAlert("Xoá loại tour không thành công", "error");
            }
            else
            {
                SetAlert("Xoá loại tour thành công", "success");
            }
            return RedirectToAction(nameof(Index));
        }

       
    }
}
