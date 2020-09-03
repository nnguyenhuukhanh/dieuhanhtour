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
using Newtonsoft.Json;

namespace dieuhanhtour.Controllers
{
    public class TuyentqController : BaseController
    {
        private readonly qltourContext _context;
        private readonly ITuyentqRepository _tuyentqRepository;
        private readonly IDiemtqRepository _diemtqRepository;

        public TuyentqController(qltourContext context,ITuyentqRepository tuyentqRepository,IDiemtqRepository diemtqRepository)
        {
            _context = context;
            _tuyentqRepository = tuyentqRepository;
            _diemtqRepository = diemtqRepository;
        }

        // GET: Tuyentq
        public IActionResult Index(string searchString, int page=1)
        {
            searchString = string.IsNullOrEmpty(searchString) ? "" : searchString;
            HttpContext.Session.SetString("urlTuyentq", UriHelper.GetDisplayUrl(Request));
            var tuyentq = _tuyentqRepository.GetTuyentq(searchString, page);
            ViewBag.tuyentq = tuyentq;
            ViewData["CurrentFilter"] = searchString;
            return View(tuyentq);
        }

       

        // GET: Tuyentq/Create
        public IActionResult Create()
        {
            Tuyentq t = new Tuyentq();
            t.Code = _tuyentqRepository.newCode();
            listDiemtq("");
            return View(t);
        }

        // POST: Tuyentq/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Code,Tuyen,Tentuyen")] Tuyentq tuyentq)
        {
            if (ModelState.IsValid)
            {
                tuyentq.Tuyen = tuyentq.Tuyen.Replace(',', '-');
                tuyentq.Tentuyen = tuyentq.Tentuyen.Replace(',', '-');
                var result = _tuyentqRepository.Create(tuyentq);
                if (result == null)
                {
                    SetAlert("Thêm tuyến tham quan không thành công", "error");
                }
                else
                {
                    SetAlert("Thêm tuyến tham quan thành công", "success");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tuyentq);
        }

        // GET: Tuyentq/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuyentq = _tuyentqRepository.GetById(id);
            if (tuyentq == null)
            {
                return NotFound();
            }
            listDiemtq("");
            return View(tuyentq);
        }

        // POST: Tuyentq/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Code,Tuyen,Tentuyen")] Tuyentq tuyentq)
        {
            if (id != tuyentq.Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                tuyentq.Tuyen = tuyentq.Tuyen.Replace(',', '-');
                tuyentq.Tentuyen = tuyentq.Tentuyen.Replace(',', '-');
                var result = _tuyentqRepository.Update(tuyentq);
                if (result == null)
                {
                    SetAlert("Cập nhật tuyến tham quan không thành công", "error");
                }
                else
                {
                    SetAlert("Cập nhật tuyến tham quan thành công", "success");
                }
                return Redirect(HttpContext.Session.GetString("urlTuyentq"));
            }
            return View(tuyentq);
        }

        // GET: Tuyentq/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuyentq = _tuyentqRepository.GetById(id);
            if (tuyentq == null)
            {
                return NotFound();
            }
            var result = _tuyentqRepository.Delete(tuyentq);
            if (result == null)
            {
                SetAlert("Xóa điểm tham quan không thành công", "error");
            }
            else
            {
                SetAlert("Xóa điểm tham quan thành công", "success");
            }
            return Redirect(HttpContext.Session.GetString("urlTuyentq"));
        }

       
        
        public void listDiemtq(string selected)
        {
            List<Dmdiemtq> diemtq = _diemtqRepository.GetAll().ToList();
            ViewBag.diemtq = new SelectList(diemtq, "Code", "Diemtq", selected);
        }
        public JsonResult getAllDiemtq()
        {
            IQueryable<Dmdiemtq> a = _diemtqRepository.getAllDiemtq();
            return Json(new
            {
                data = JsonConvert.SerializeObject(a),
                status = true
            });
        }
        [HttpGet]
        public JsonResult getTuyentqByCode(string code)
        {
            var t = _tuyentqRepository.GetById(code);
            return Json(t);
        }
    }
}
