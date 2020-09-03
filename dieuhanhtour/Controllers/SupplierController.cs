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
using dieuhanhtour.ViewModel;

namespace dieuhanhtour.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IQuocgiaRepository _quocgiaRepository;
        private readonly IThanhphoRepository _thanhphoRepository;

        public SupplierController(ISupplierRepository supplierRepository,
                                   IQuocgiaRepository quocgiaRepository, IThanhphoRepository thanhphoRepository)
        {
            _supplierRepository = supplierRepository;
            _quocgiaRepository = quocgiaRepository;
            _thanhphoRepository = thanhphoRepository;
        }


        // GET: Supplier
        public IActionResult Index(string searchString, int? page)
        {
            searchString = string.IsNullOrEmpty(searchString) ? "" : searchString;
            HttpContext.Session.SetString("urlSupplier", UriHelper.GetDisplayUrl(Request));
            var supplier = _supplierRepository.ListSupplier(searchString, page);
            ViewBag.supplier = supplier;
            ViewData["CurrentFilter"] = searchString;
            return View(supplier);
        }

       

        // GET: Supplier/Create
        public IActionResult Create()
        {
            Supplier s = new Supplier();
            s.Code = _supplierRepository.nextSupplierCode();
            listQuocgia("VIETNAM");
            listThanhpho("HO CHI MINH");
            Trangthai(true);
            return View(s);
        }

        // POST: Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Code,Codecn,Tengiaodich,Tenthuongmai,Thanhpho,Quocgia,Diachi,Dienthoai,Fax,Masothue,Nganhnghe,Nguoilienhe,Website,Email,Trangthai,Ngaytao,Nguoitao,Chinhanh")] Supplier model)
        {
            if (ModelState.IsValid)
            {
                model.Chinhanh = HttpContext.Session.GetString("chinhanh");
                var result = _supplierRepository.CreateSupplierVM(model, HttpContext.Session.GetString("hoten"));
                if (result == null)
                {
                    SetAlert("Thêm nhà cung cấp không thành công", "error");
                }
                else
                {
                    SetAlert("Thêm nhà cung cấp thành công", "success");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Supplier/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = _supplierRepository.getSupplierById(id);// await _context.Supplier.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }        
            if (supplier.Chinhanh!= HttpContext.Session.GetString("chinhanh"))
            {
                SetAlert("Bạn không có quyền cập nhật nhà cung cấp này", "error");
                return Redirect(HttpContext.Session.GetString("urlSupplier"));
            }
            else
            {
                SupplierViewModel model = new SupplierViewModel
                {
                    Code = supplier.Code,
                    Codecn = supplier.Codecn,
                    Tengiaodich = string.IsNullOrEmpty(supplier.Tengiaodich) ? "" : supplier.Tengiaodich,
                    Tenthuongmai = string.IsNullOrEmpty(supplier.Tenthuongmai) ? "" : supplier.Tenthuongmai,
                    Thanhpho = supplier.Thanhpho,
                    Quocgia = supplier.Quocgia,
                    Diachi = string.IsNullOrEmpty(supplier.Diachi) ? "" : supplier.Diachi,
                    Dienthoai = string.IsNullOrEmpty(supplier.Dienthoai) ? "" : supplier.Dienthoai,
                    Fax = string.IsNullOrEmpty(supplier.Fax) ? "" : supplier.Fax,
                    Masothue = string.IsNullOrEmpty(supplier.Masothue)?"":supplier.Masothue,
                    Nganhnghe = string.IsNullOrEmpty( supplier.Nganhnghe)?"":supplier.Nganhnghe,
                    Nguoilienhe = string.IsNullOrEmpty( supplier.Nguoilienhe)?"":supplier.Nguoilienhe,
                    Website = string.IsNullOrEmpty(supplier.Website)?"":supplier.Website,
                    Email = string.IsNullOrEmpty(supplier.Email)?"":supplier.Email,
                    Trangthai = supplier.Trangthai,
                    Chinhanh = HttpContext.Session.GetString("macn")
                };
                listQuocgia(model.Quocgia);
                listThanhpho(model.Thanhpho);
                Trangthai(model.Trangthai);               
                return View(model);
            }
        }

        // POST: Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Code,Codecn,Tengiaodich,Tenthuongmai,Thanhpho,Quocgia,Diachi,Dienthoai,Fax,Masothue,Nganhnghe,Nguoilienhe,Website,Email,Trangthai,Ngaytao,Nguoitao,Chinhanh")] SupplierViewModel model)
        {
            if (id != model.Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _supplierRepository.UpdateSupplierVM(model, HttpContext.Session.GetString("hoten"));
                if (result == null)
                {
                    SetAlert("Cập nhật supplier không thành công.", "error");
                }
                else
                {
                    SetAlert("Cập nhật supplier thành công.", "success");
                }
                return Redirect(HttpContext.Session.GetString("urlSupplier"));
            }
            return View(model);
        }

        // GET: Supplier/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = _supplierRepository.getSupplierById(id);// await _context.Supplier.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            if (supplier.Chinhanh != HttpContext.Session.GetString("chinhanh"))
            {
                SetAlert("Bạn không có quyền xoá nhà cung cấp này", "error");
                return Redirect(HttpContext.Session.GetString("urlSupplier"));
            }
            else
            {
                int result = _supplierRepository.DelSupplier(supplier.Code);
                if (result > 0)
                {
                    SetAlert("Xoá nhà cung cấp thành công", "success");
                }
                else
                {
                    SetAlert("Xoá nhà cung cấp không thành công", "error");
                }
                return Redirect(HttpContext.Session.GetString("urlSupplier"));
            }           
        }

        

        public void listQuocgia(string selected)
        {
            List<Quocgia> quocgia = _quocgiaRepository.ListQuocgia().ToList();
            ViewBag.quocgia = new SelectList(quocgia, "TenNuoc", "TenNuoc", selected);
        }
        public void listThanhpho(string selected)
        {
            List<Thanhpho> thanhpho = _thanhphoRepository.ListThanhpho().ToList();
            ViewBag.thanhpho = new SelectList(thanhpho, "Tentp", "Tentp", selected);
        }
        public void Trangthai(bool select)
        {
            List<SelectListItem> trangthai = new List<SelectListItem>();
            trangthai.Add(new SelectListItem { Text = "Kích hoạt", Value = "True" });
            trangthai.Add(new SelectListItem { Text = "Khóa", Value = "False" });

            ViewBag.trangthai = new SelectList(trangthai, "Value", "Text", select);
        }
        public JsonResult dsSupplier()
        {
            var data = _supplierRepository.ListSupplier().ToList();
            return Json( new { data=data });
        }
    }
}
