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
    public class DiemtqController : BaseController
    {
        private readonly IDiemtqRepository _diemtqRepository;
        private readonly IThanhphoRepository _thanhphoRepository;
        private readonly ISupplierRepository _supplierRepository;
        string temp = "", log = "";
        public DiemtqController(IDiemtqRepository diemtqRepository, IThanhphoRepository thanhphoRepository, ISupplierRepository supplierRepository)
        {
            _diemtqRepository = diemtqRepository;
            _thanhphoRepository = thanhphoRepository;
            _supplierRepository = supplierRepository;
        }

        // GET: Diemtq
        public IActionResult Index(string searchString, int page = 1)
        {
            searchString = string.IsNullOrEmpty(searchString) ? "" : searchString;
            HttpContext.Session.SetString("urlDiemtq", UriHelper.GetDisplayUrl(Request));
            var diemtq = _diemtqRepository.GetDiemtq(searchString, page);
            listTinh("");
            ViewBag.diemtq = diemtq;
            ViewData["CurrentFilter"] = searchString;
            return View(diemtq);
        }

        #region Thêm điềm tham quan
        // GET: Diemtq/Create
        public IActionResult Create()
        {
            Dmdiemtq d = new Dmdiemtq();
            listTinh("");
            listThanhphoByTinh("ANG","");
            d.Code = newCode("ANG");
            listSupplier("");
            d.Giave = 0;
            d.Giatreem = 0;
            d.Vatra = 10;
            d.Vatvao = 10;
            d.Tilelai = 0;
            return View(d);
        }

        // POST: Diemtq/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Dmdiemtq dmdiemtq)
        {
            if (ModelState.IsValid)
            {
                var result = _diemtqRepository.Create(dmdiemtq);
                if (result != null)
                {
                    SetAlert("Thêm điểm tham quan thành công", "success");
                }
                else
                {
                    SetAlert("Thêm điểm tham quan không thành công", "Error");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dmdiemtq);
        }
        #endregion
        #region Cập nhật điểm tham quan
        // GET: Diemtq/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dmdiemtq = await _diemtqRepository.GetByIdAsync(id);
            if (dmdiemtq == null)
            {
                return NotFound();
            }
            listTinh(dmdiemtq.Tinhtp);
            listThanhphoByTinh(dmdiemtq.Tinhtp,dmdiemtq.Thanhpho);
            listSupplier(dmdiemtq.Congno);
            return View(dmdiemtq);
        }

        // POST: Diemtq/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Dmdiemtq dmdiemtq)
        {
            temp = ""; log = "";
            if (id != dmdiemtq.Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Dmdiemtq dtq = _diemtqRepository.GetById(id);
                if (dtq.Thanhpho != dmdiemtq.Thanhpho)
                {
                    temp += String.Format("- Thành phố thay đổi: {0}->{1}", dtq.Thanhpho, dmdiemtq.Thanhpho);
                }
                if (dtq.Diemtq != dmdiemtq.Diemtq)
                {
                    temp += String.Format("- Điểm tham quan thay đổi: {0}->{1}", dtq.Diemtq, dmdiemtq.Diemtq);
                }
                if (dtq.Giave != dmdiemtq.Giave)
                {
                    temp += String.Format("- Giá vé thay đổi: {0:#,##0}->{1:#,##0}", dtq.Giave, dmdiemtq.Giave);
                }
                if (dtq.Congno != dmdiemtq.Congno)
                {
                    temp += String.Format("- Công nợ thay đổi: {0}->{1}", dtq.Congno, dmdiemtq.Congno);
                }
                if (dtq.Vatra != dmdiemtq.Vatra)
                {
                    temp += String.Format("- VAT ra thay đổi: {0}->{1}", dtq.Vatra, dmdiemtq.Vatra);
                }
                if (dtq.Vatvao != dmdiemtq.Vatvao)
                {
                    temp += String.Format("- VAT vào thay đổi: {0}->{1}", dtq.Vatvao, dmdiemtq.Vatvao);
                }
                if (dtq.Tilelai != dmdiemtq.Tilelai)
                {
                    temp += String.Format("- Tỷ lệ lãi thay đổi: {0}->{1}", dtq.Tilelai, dmdiemtq.Tilelai);
                }
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật điểm tham quan: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString();
                    dtq.logfile = dtq.logfile + log;
                }
                dtq.Thanhpho = dmdiemtq.Thanhpho;
                dtq.Diemtq = dmdiemtq.Diemtq;
                dtq.Giave = dmdiemtq.Giave;
                dtq.Giatreem = dmdiemtq.Giatreem;
                dtq.Congno = dmdiemtq.Congno;
                dtq.Vatvao = dmdiemtq.Vatvao;
                dtq.Vatra = dmdiemtq.Vatra;
                dtq.Tilelai = dmdiemtq.Tilelai;
                var result = _diemtqRepository.Update(dtq);
                if (result == null)
                {
                    SetAlert("Cập nhật điểm tham quan không thành công", "error");
                }
                else
                {
                    SetAlert("Cập nhật điểm tham quan thành công", "success");
                }

                return Redirect(HttpContext.Session.GetString("urlDiemtq"));
            }
            return View(dmdiemtq);
        }
        #endregion
        #region Xoá điểm tham quan
        // GET: Diemtq/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dmdiemtq = _diemtqRepository.GetById(id);
            if (dmdiemtq == null)
            {
                return NotFound();
            }
            try
            {
                var result = _diemtqRepository.Delete(dmdiemtq);
                SetAlert("Xóa điểm tham quan thành công.", "success");
            }
            catch
            {
                SetAlert("Điểm tham quan này đang tồn tại trong các chương trình tour, không thể xoá", "error");
            }
           
            return Redirect(HttpContext.Session.GetString("urlDiemtq"));
        }

        #endregion
        #region List box

        public void listThanhpho(string selected)
        {
            List<Thanhpho> thanhpho = _thanhphoRepository.ListThanhpho().ToList();
            var empty = new Thanhpho
            {
                Matp = "",
                Tentp = "-- Chưa có thông tin --"
            };
            thanhpho.Insert(0, empty);
            ViewBag.thanhpho = new SelectList(thanhpho, "Matp", "Tentp", selected);
        }

        public string newCode(string matinh)
        {
            try
            {
                var a = _diemtqRepository.newCode(matinh);
                return a;
            }
            catch { return ""; }

        }
        [HttpGet]
        public ActionResult GetThanhphoByTinh(string matinh)
        {
            List<Thanhpho> thanhphos = new List<Thanhpho>();
            if (!string.IsNullOrWhiteSpace(matinh))
            {              
                thanhphos = _thanhphoRepository.ListThanhphoByTinh(matinh).ToList();
                if (thanhphos.Count() == 0)
                {
                    var empty = new Thanhpho
                    {
                        Matp = "",
                        Tentp = "Không có thông tin"
                    };
                    thanhphos.Insert(0, empty);
                }
                else
                {
                    var empty = new Thanhpho
                    {
                        Matp = "",
                        Tentp = "-- Chọn thông tin --"
                    };
                    thanhphos.Insert(0, empty);
                }
            }

            return Json(thanhphos);

        }

        public void listThanhphoByTinh(string matinh,string selected)
        {
            List<Thanhpho> thanhpho = _thanhphoRepository.ListThanhphoByTinh(matinh).ToList();
            var empty = new Thanhpho
            {
                Matp = "",
                Tentp = "-- Chọn thông tin --"
            };
            thanhpho.Insert(0, empty);
            ViewBag.thanhpho = new SelectList(thanhpho, "Matp", "Tentp", selected);
        }
        public void listTinh(string selected)
        {
            List<vTinh> tinh = _thanhphoRepository.ListTinh().ToList();
            ViewBag.tinh= new SelectList(tinh, "Matinh", "Tentinh", selected);
        }

        public ActionResult GetDiemtqById(string code)
        {
            var diemtqs = _diemtqRepository.GetById(code);
            return Json(diemtqs);
        }
        public void listDiemtq(string selected)
        {
            List<Dmdiemtq> lst = _diemtqRepository.GetLstDiemtq().ToList();
            var empty = new Dmdiemtq
            {
                Code = "",
                Diemtq = "-- Chọn thông tin --"
            };
            lst.Insert(0, empty);
            ViewBag.dtq = new SelectList(lst, "Code", "Diemtq", selected);
        }
        public void listSupplier(string selected)
        {
            List<Supplier> congno = _supplierRepository.ListSupplier().ToList();
            var empty = new Supplier
            {
                Code = "",
                Tengiaodich = "-- Chọn thông tin --"
            };
            congno.Insert(0, empty);
            ViewBag.supplier = new SelectList(congno, "Code", "Tengiaodich", selected);
        }
        #endregion
    }
}
